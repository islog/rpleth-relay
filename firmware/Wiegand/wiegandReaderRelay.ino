/**
 * \file wiegandReaderRelay.ino
 * \author Guillaume R. <guillaume-dev@islog.com>
 * \brief Firmware Rpleth-relay with wiegand reader. 
 */

#include "MyConst.h"
#include <SdFat.h>
#include "MyWiegand.h"
#include "MyArduino.h"
#include "MyHid.h"
#include "MyLcd.h"
#include "MyConst.h"
#include <LiquidCrystal.h>
#include <EEPROM.h>
#include <SPI.h>
#include <Ethernet.h>
#include <avr/wdt.h>

byte display_time = 3;
uint32_t blink = 0;
uint32_t scroll = 0;
EthernetServer server = EthernetServer(23);

void setup()
{
	Serial.begin (9600);
	// for old circuit
	lcd.begin (16, 2, 4, 5, 6, 7, 8, 9);
	// for new circuit
	//lcd.begin (16, 2, 4, 10, 5, 6, 7, 8, 9);
	uint32_t initialisation = 0;
	uint64_t tmp = 0;
	hid.init ();
	wiegand.init();
	if (arduino.read() == 0)
	{
		arduino.init();
	}
	init_ethernet ();
	initialisation = millis ();
	lcd.print ("Initialisation..");
	lcd.print (Ethernet.localIP());
	wiegand.reset ();
	while (millis() - initialisation <= 15000)
	{
		if (wiegand.available () == 1)
		{
			if (tmp == wiegand.bitHolder && tmp != 0)
			{
				arduino.init ();
				initialisation = millis() - 15001;
				lcd.clearPrint ("Configuration reset");
				delay (100);
			}
			else
			{
				tmp = wiegand.bitHolder;
			}
			wiegand.reset ();
		}
	}
	lcd.clearPrint (arduino.ard.message);
}

void init_ethernet ()
{
	if (arduino.ard.dhcp == 1)
	{
		Ethernet.begin(arduino.ard.mac);
	}
	else
	{
		Ethernet.begin (arduino.ard.mac, arduino.ard.ip, arduino.ard.dns, arduino.ard.gateway, arduino.ard.subnet);
	}
}

void proc_cmd_rpleth (byte * com, byte cmd, byte * data)
{
	switch (cmd)
	{
		case STATEDHCP:
			break;
		case DHCP:
			if (arduino.ard.dhcp == 0)
				arduino.ard.dhcp = 1;
			else
				arduino.ard.dhcp = 0;
			break;
		case MAC:
			for (int i = 0; i < 4; i++)
			{
				arduino.ard.mac [i] = data [i];
			}
			break;
		case IP:
			for (int i = 0; i < 6; i++)
			{
				arduino.ard.ip [i] = data [i];
			}
		case SUBNET:
			for (int i = 0; i < 4; i++)
			{
				arduino.ard.subnet [i] = data [i];
			}
			break;
		case GATEWAY:
			for (int i = 0; i < 4; i++)
			{
				arduino.ard.gateway [i] = data [i];
			}
			break;
		case PORT:
			arduino.ard.port = data [0];
			arduino.ard.port <<= 4;
			arduino.ard.port = data [1];
			break;
		case MESSAGE:
			for (int i = 0; i < com[2] && i < 16; i++)
			{
				arduino.ard.message [i] = data [i];
			}
			arduino.ard.message[min (com[2], 31)] = '\0';
			lcd.clearPrint (arduino.ard.message);
			break;
		case RESET:
			free (data);
			free (com);
			reset ();
			break;
	}
}

void proc_cmd_hid (byte cmd, byte * data, byte size)
{
	switch (cmd)
	{
		case BEEP:	
			hid.bip (data[0]);
			break;
		case BLINKLED1:
			hid.blink_led1 (data[0]);
			break;
		case BLINKLED2:
			hid.blink_led2 (data[0]);
			break;
		case NOP:
			break;
		case BADGE:
			break;
	}
}

void proc_cmd_lcd (byte cmd, byte * data, byte size)
{
	switch (cmd)
	{
		case DISPLAYS:
			aff_lcd (data, size, DISPLAYS);
			break;
		case DISPLAYT:
			aff_lcd (data, size, DISPLAYS);
			break;
		case BLINK: 
			if (lcd.is_blink == 0)
				lcd.is_blink = 1;
			else
				lcd.is_blink = 0;
			lcd.display ();
			break;
		case SCROLL: 
			if (lcd.is_scroll == 0)
				lcd.is_scroll = 1;
			else
				lcd.is_scroll = 0;
			break;
		case DISPLAYTIME: 
			display_time = data[0];
			break;
	}
}

void proc_communication ()
{
	EthernetClient client = server.available();
	if (client)
	{
		byte * cmd = NULL;
		byte * data = NULL;
		byte checksum;
		byte statut;
		cmd = receive_cmd (client);
		data = receive_cmd_data (client, cmd[2]);
		checksum = client.read ();
		statut = check_checksum (cmd, data, checksum);
		if (statut == 1)
		{
			if (cmd[0] > LCD)
			{
				statut = BADDEVICE;
			}
			else if ((cmd[0] == RPLETH && cmd[1] > RESET) || (cmd[0] == HID && cmd[1] > BADGE) || (cmd[0] == LCD && cmd[1] > DISPLAYTIME))
			{
				statut = ECHEC;
			}
			else
			{
				statut = SUCCES;
			}
			if (cmd[0] == RPLETH && cmd[1] == STATEDHCP)
			{
				free (data);
				data = (byte *)malloc (sizeof (byte));
				data[0] = arduino.ard.dhcp;
				answer_data (cmd, SUCCES, data, 0x01, client);
			}
			else
			{
				answer (cmd, statut, client);
			}
			if (cmd [0] == RPLETH)
			{
				proc_cmd_rpleth (cmd, cmd[1], data);
			}
			else if (cmd [0] == HID)
			{
				proc_cmd_hid (cmd [1], data, cmd[2]);
			}
			else if (cmd [0] == LCD)
			{
				proc_cmd_lcd (cmd [1], data, cmd [2]);
			}
		}
		else
		{
			answer (cmd, BADCHECKSUM, client);
		}
		free (cmd);
		free (data);
	}
}

void loop()
{
	if (millis () - blink > 1000 && lcd.is_blink != 0)
	{
		blink = millis ();
		lcd.blink ();
	}
	if (millis () - scroll > 500 && lcd.is_scroll != 0)
	{
		if (lcd.displayed () == 1)
		{
			scroll = millis ();
			lcd.scroll ();
		}
	}
	if (millis () - wiegandTimeout > 100 && wiegand.bitCount > 0 && wiegand.available () != 1)
	{
		// check if it noise or trame
		if (wiegand.bitCount > 10)
			answer_badge ();
		wiegandTimeout = millis ();
		wiegand.reset();
	}
	proc_communication ();
}

void aff_lcd (byte * data, byte size, byte mode)
{
	lcd.clear ();
	if (mode == DISPLAYS)
	{
		lcd.print (data, size);
		delay (display_time*1000);
		lcd.clearPrint (arduino.ard.message);
	}
	else
	{
		lcd.print (data, size-1);
		if (data [size-1] != 0x00)
		{
			delay (data [size-1]*1000);
			lcd.clearPrint (arduino.ard.message);
		}
	}
}

byte check_checksum (byte * cmd, byte * data, byte checksum)
{
	byte result = 0, tmp = 0;
	for (int i = 0; i < 3; i++)
	{
		tmp ^= cmd [i];
	}
	for (int i = 0; i < cmd[2]; i++)
	{
		tmp ^= data [i];
	}
	if (tmp == checksum)
	{
		result = 1;
	}
	return result;
}

void answer (byte * com, byte statut, EthernetClient client)
{
	answer_data (com, statut, NULL, 0, client);
}

void answer (byte * com, byte statut)
{
	answer_data (com, statut, NULL, 0);
}

void answer_data (byte * com, byte statut, byte * data, byte size, EthernetClient client)
{
	byte checksum = 0;
	client.write (statut);
	checksum ^= statut;
	for (int i = 0; i < 2; i++)
	{
		client.write (com[i]);
		checksum ^= com[i];
	}
	client.write (size);
	checksum ^= size;
	for (int i = 0; i < size; i++)
	{
		client.write (data[i]);
		checksum ^= data[i];
	}
	client.write (checksum);
}

void answer_data (byte * com, byte statut, byte * data, byte size)
{
	byte checksum = 0;
	server.write (statut);
	checksum ^= statut;
	for (int i = 0; i < 2; i++)
	{
		server.write (com[i]);
		checksum ^= com[i];
	}
	server.write (size);
	checksum ^= size;
	for (int i = 0; i < size; i++)
	{
		server.write (data[i]);
		checksum ^= data[i];
	}
	server.write (checksum);
}

void answer_badge ()
{
	// wait for receive full trame
	delay (100);
	byte * cmd = (byte *)malloc (2 * sizeof (byte));
	byte size = wiegand.bitCount/8;
	if (wiegand.bitCount%8 != 0)
		size ++;
	byte * data = (byte *)malloc (size * sizeof (byte));
	byte statut = SUCCES;
	for (int i = 0, j = wiegand.bitCount-8; i < size; i++, j -= 8)
	{
		data[i] = (wiegand.bitHolder >> j) & 0xff;
	}
	cmd[0] = HID;
	cmd[1] = BADGE;
	answer_data (cmd, statut, data, size);
	free (data);
	free (cmd);
}

byte * receive_cmd (EthernetClient client)
{
	byte * com = (byte *)malloc (3 * sizeof (byte));
	for (int i = 0; i < 3; i++)
	{
		com [i] = client.read ();
		client = server.available();
	}
	return com;
}

byte * receive_cmd_data (EthernetClient client, byte size)
{
	byte * cmd = NULL;
	if (size != 0)
		 cmd = (byte *)malloc (size * sizeof (byte));
	for (int i = 0; i < size; i++)
	{
		if (client)
		{
			cmd [i] = client.read ();
		}
		else
		{
			i --;
		}
		client = server.available ();
	}
	return cmd;
}

void reset ()
{
	cli();
	wdt_enable(WDTO_15MS);
	while(1);
}

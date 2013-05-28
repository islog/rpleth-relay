#include "MyRs.h"
#include "MyArduino.h"
#include "MyHid.h"
#include "MyLcd.h"
#include "MyRs.h"
#include <LiquidCrystal.h>
#include <EEPROM.h>
#include <SPI.h>
#include <Ethernet.h>
#include <SdFat.h>
#include <avr/wdt.h>
#include <SoftwareSerial.h>

byte display_time = 3;
uint32_t blink = 0;
uint32_t scroll = 0;
EthernetServer server = EthernetServer(23);

void setup()
{
	Serial.begin (9600);
	rs.begin();
	// for old circuit
	lcd.begin (16, 2, 4, 5, 6, 7, 8, 9);
	// for new circuit
	//lcd.begin (16, 2, 4, 10, 5, 6, 7, 8, 9);
	hid.init ();
	// change to get the conf from SD
	if (arduino.read() == 0)
	{
		arduino.init();
	}
	init_ethernet ();
	//lcd.print ("Initialisation..");
	Serial.println (Ethernet.localIP());
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

byte proc_cmd_arduino (byte * com, byte cmd, byte * data)
{
	byte result = 0;
	switch (cmd)
	{
		case 0x01:
			if (arduino.ard.dhcp == 0)
				result = 0x05;
			else
				result = 0x06;
			break;
		case 0x02:
			if (arduino.ard.dhcp == 0)
				arduino.ard.dhcp = 1;
			else
				arduino.ard.dhcp = 0;
			break;
		case 0x03:
			for (int i = 0; i < 4; i++)
			{
				arduino.ard.ip [i] = data [i];
			}
			break;
		case 0x04:
			for (int i = 0; i < 6; i++)
			{
				arduino.ard.mac [i] = data [i];
			}
		case 0x05:
			for (int i = 0; i < 4; i++)
			{
				arduino.ard.subnet [i] = data [i];
			}
			break;
		case 0x06:
			for (int i = 0; i < 4; i++)
			{
				arduino.ard.gateway [i] = data [i];
			}
			break;
		case 0x07:
			arduino.ard.port = data [0];
			arduino.ard.port <<= 4;
			arduino.ard.port = data [1];
			break;
		case 0x08:
			for (int i = 0; i < com[2] && i < 16; i++)
			{
				arduino.ard.message [i] = data [i];
			}
			arduino.ard.message[min (com[2], 31)] = '\0';
			lcd.clearPrint (arduino.ard.message);
			break;
		case 0x09:
			break;
		case 0x0A:
			free (data);
			free (com);
			reset ();
			break;
		default:
			result = 1;
			break;
	}
	return result;
}

byte proc_cmd_hid (byte cmd, byte * data, byte size)
{
	byte result = 0;
	switch (cmd)
	{
		case 0x00:	
			hid.bip (data[0]);
			break;
		case 0x01:
			hid.blink_led1 (data[0]);
			break;
		case 0x02:
			hid.blink_led2 (data[0]);
			break;
		case 0x03:
			break;
		case 0x04:
			break;
		case 0x05:
			break;
		default:
			lcd.clearPrint ("bug");
			result = 1;
			break;
	}
	return result;
}

byte proc_cmd_lcd (byte cmd, byte * data, byte size)
{
	byte result = 0;
	switch (cmd)
	{
		case 0x00: 
			aff_lcd (data, size, 0x00);
			break;
		case 0x01:
			aff_lcd (data, size, 0x01);
			break;
		case 0x02: 
			if (lcd.is_blink == 0)
				lcd.is_blink = 1;
			else
				lcd.is_blink = 0;
			lcd.display ();
			break;
		case 0x03: 
			if (lcd.is_scroll == 0)
				lcd.is_scroll = 1;
			else
				lcd.is_scroll = 0;
			break;
		case 0x04: 
			display_time = data[0];
			break;
		default:
			result = 1;
			break;
	}
	return result;
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
			if (cmd[0] > 0x02)
			{
				statut = 0x05;
			}
			else if ((cmd[0] == 0x00 && cmd[1] > 0x0A) || (cmd[0] == 0x01 && cmd[1] > 0x05) || (cmd[0] == 2 && cmd[1] > 0x04))
			{
				statut = 0x01;
			}
			else
			{
				statut = 0x00;
			}
			if (cmd[0] == 0x00 && cmd[1] == 0x01)
			{
				free (data);
				data = (byte *)malloc (sizeof (byte));
				data[0] = arduino.ard.dhcp;
				answer_data (cmd, 0x00, data, 0x01, client);
			}
			else if (cmd[0] == 0x01 && cmd[1] == 0x05)
			{
				rs.send (data, cmd[2]);
				byte size;
				data = rs.receive(&size);
				answer_data (cmd, 0x00, data, size, client);
			}
			else
			{
				answer (cmd, statut, client);
			}
			if (cmd [0] == 0x00)
			{
				statut = proc_cmd_arduino (cmd, cmd[1], data);
			}
			else if (cmd [0] == 0x01)
			{
				statut = proc_cmd_hid (cmd [1], data, cmd[2]);
			}
			else if (cmd [0] == 0x02)
			{
				statut = proc_cmd_lcd (cmd [1], data, cmd [2]);
			}
		}
		else
		{
			statut = 0x02;
			answer (cmd, statut, client);
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
	proc_communication ();
}

void aff_lcd (byte * data, byte size, byte mode)
{
	lcd.clear ();
	if (mode == 0x00)
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


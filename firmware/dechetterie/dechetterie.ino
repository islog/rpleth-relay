/**
 * \file dechetterie.ino
 * \author Guillaume R. <guillaume.rodrigues@etu.unistra.fr>
 * \brief Counting software with wiegand communication. 
 */

#include "MyUrl.h"
#include <Ethernet.h>
#include <SPI.h>
#include <SdFat.h>
#include <SdBaseFile.h>
#include "MyWiegand.h"
#include "MyArduino.h"

EthernetClient client;

void init_ethernet ()
{
	if (arduino.ard.dhcp == 1)
		Ethernet.begin(arduino.ard.mac);
	else
		Ethernet.begin(arduino.ard.mac, arduino.ard.ip, arduino.ard.dns, arduino.ard.gateway, arduino.ard.subnet);
}

void setup()
{
	Serial.begin (9600);
	wiegand.init();
	if (arduino.read() == 0)
	{
		arduino.init();
	}
	url.init();
  	if (url.read() == 0)
	{
		Serial.println ("Bad url file");
	}
	init_ethernet();
	Serial.println ("Pret");
}

void loop()
{
	if (wiegand.available() > 20)
	{
		// wait the full frame
		delay (100);
		int size = strlen (url.url);
		char serial_number [20], end_url [64], get_url[88];
                char iphost [16];
                char strhost [22];
	        sprintf (iphost, "%d.%d.%d.%d", url.ip[0], url.ip[1], url.ip[2], url.ip[3]);
                sprintf (strhost, "Host: %s", iphost);
		sprintf (serial_number, "%ld", wiegand.bitHolder);
		int ind_cur = 0;
		while (url.url[ind_cur] != '[' && url.url[ind_cur] != '\0')
		{
			ind_cur++;
		}
		if (url.url[ind_cur] != '\0')
		{
			// get end of url
			if (size > ind_cur + 3)
			{
				for (int i = ind_cur+3, ind_cur_end_url = 0; i < size; i++, ind_cur_end_url++)
				{
					end_url[ind_cur_end_url] = url.url[i];
					if (i == size -1)
						end_url[ind_cur_end_url+1] = '\0';
				}
			}
			else
				end_url[0] = '\0';

			// delete it
			url.url[ind_cur] = '\0';
			// add badge number
			strcat (url.url, serial_number);
			// add end of url
			strcat (url.url, end_url);
                        sprintf(get_url, "GET %s HTTP/1.1", url.url);
			Serial.println (get_url);
			if (client.connect (iphost,80))
                        {
				Serial.println ("Connection successful, sending request...");
                                client.println(get_url);
                                client.println(strhost);
                                client.println("Connection: close");
                                client.println();
                        }
			else
				Serial.println ("Connection fail");
			client.stop ();
			if (url.read() == 0)
			{
				Serial.println ("Bad url file");
			}
		}
		else
			Serial.println ("Bad url, missing '['");
		wiegand.reset();
	}
	// timeout receive wiegand
	if (millis () - wiegandTimeout > 150 && wiegand.bitCount > 0)
	{
		Serial.println ("reset");
		wiegand.reset();
	}
}

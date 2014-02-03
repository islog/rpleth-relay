#include <EEPROM.h>
#include "MyArduino.h"

void MyArduino::init()
{
/*    Serial.println ("RESET EEPROM");
   for (size_t i = 0; i < sizeof(Arduino); i++)
   EEPROM.write(i, 0xff);*/

  if (EEPROM.read(0) == 0xff)
  {
    ard.mac [0] = 0x90;
    ard.mac [1] = 0xA2;
    ard.mac [2] = 0xDA;
    ard.mac [3] = 0x0D;
    ard.mac [4] = 0x36;
    ard.mac [5] = 0x80;
    ard.ip [0] = 10;
    ard.ip [1] = 2;
    ard.ip [2] = 0;
    ard.ip [3] = 104;
    ard.dns [0] = 10;
    ard.dns [1] = 2;
    ard.dns [2] = 255;
    ard.dns [3] = 254;
    ard.subnet [0] = 255;
    ard.subnet [1] = 255;
    ard.subnet [2] = 0;
    ard.subnet [3] = 0;
    ard.gateway [0] = 10;
    ard.gateway [1] = 2;
    ard.gateway [2] = 255;
    ard.gateway [3] = 254;
    ard.port = 9559;
    ard.dhcp = 0;

    writeConfiguration();
  }
  else
  {
    readConfiguration();
  }
  strcpy (ard.message,"Pret");
}

void MyArduino::readConfiguration ()
{
  char *p = (char *)&ard;
  for (size_t i = 0; i < sizeof(Arduino); i++)
    p[i] = EEPROM.read(i);
}

void MyArduino::writeConfiguration ()
{
  char *p = (char *)&ard;
  for (size_t i = 0; i < sizeof(Arduino); i++)
    EEPROM.write(i, p[i]);
}

MyArduino arduino;




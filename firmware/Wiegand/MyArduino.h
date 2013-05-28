#ifndef _MYCONF_H
#define _MYCONF_H

#include "Arduino.h"
//#include "SD.h"
#include "SdFat.h"

typedef struct str_arduino
{
	byte ip [4];
	byte dns [4];
	byte subnet [4];
	byte gateway [4];
	char message [32];
	byte mac [6];
	uint16_t port;
	byte dhcp;
}Arduino;

class MyArduino
{
 private:

 public:
	void init();
	uint8_t read ();
	Arduino ard;
	SdFat sd;
};

extern MyArduino arduino;

#endif

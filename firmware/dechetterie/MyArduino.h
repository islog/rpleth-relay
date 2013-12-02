/**
 * \file MyArduino.h
 * \author Guillaume R. <guillaume.rodrigues@etu.unistra.fr>
 * \brief Handle arduino communication for Rpleth-relay. 
 */

#ifndef _MYARDUINO_H
#define _MYARDUINO_H

#include "Arduino.h"
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

#endif /* _MYARDUINO_H */

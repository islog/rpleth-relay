/**
 * \file MyWiegand.cpp
 * \author Guillaume R. <guillaume-dev@islog.com>
 * \brief Handle wiegand communication for Rpleth-relay. 
 */
 
#include "MyWiegand.h"

void MyWiegand::init()
{
	Serial.begin(9600);
	reset ();
	// for Arduino ethernet
	attachInterrupt (0, Data0, RISING);
	attachInterrupt (1, Data1, RISING);
	// for Arduino Mega+shield ethernet
	//attachInterrupt (2, Data0, RISING);
	//attachInterrupt (3, Data1, RISING);
}

void MyWiegand::reset()
{
	bitHolder = 0;
	bitCount = 0;
	wiegandTimeout = millis();
}

uint8_t MyWiegand::available ()
{
	return bitCount;
}

MyWiegand wiegand;

void Data0()
{
        Serial.println ("Data0");
	wiegandTimeout = millis ();
	wiegand.bitCount ++;
	wiegand.bitHolder = wiegand.bitHolder << 1;
}

void Data1 ()
{
        Serial.println ("Data1");
	wiegandTimeout = millis ();
	wiegand.bitCount ++;
	wiegand.bitHolder = wiegand.bitHolder << 1;
	wiegand.bitHolder |= 1;
}


uint32_t wiegandTimeout = 0;

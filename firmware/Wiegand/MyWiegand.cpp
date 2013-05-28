/**
 * \file MyWiegand.cpp
 * \author Guillaume R. <guillaume-dev@islog.com>
 * \brief Handle wiegand communication for Rpleth-relay. 
 */
 
#include "MyWiegand.h"

void MyWiegand::init()
{
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
}

uint8_t MyWiegand::available ()
{
	return bitCount;
}

uint8_t MyWiegand::read ()
{
	uint8_t result = 1;
	ifstream file("wiegand");
	if (!file.is_open()) 
		result = 0;
	char *p = (char *)&wieg;
	for (size_t i=0; i<sizeof (Wiegand); i++)
		p[i] = file.get();
	file.close();
	return result;
}

MyWiegand wiegand;

void Data0()
{
	wiegandTimeout = millis ();
	wiegand.bitCount ++;
	wiegand.bitHolder = wiegand.bitHolder << 1;
}

void Data1 ()
{
	wiegandTimeout = millis ();
	wiegand.bitCount ++;
	wiegand.bitHolder = wiegand.bitHolder << 1;
	wiegand.bitHolder |= 1;
}


uint32_t wiegandTimeout = 0;

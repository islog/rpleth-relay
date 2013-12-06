/**
 * \file MyWiegand.h
 * \author Guillaume R. <guillaume-dev@islog.com>
 * \brief Handle wiegand communication for Rpleth-relay. 
 */

#ifndef _MYWIEGAND_h
#define _MYWIEGAND_h

#include "Arduino.h"

class MyWiegand
{
	private:

	public:
		uint64_t bitHolder;
		uint8_t bitCount;
		void init();
		void reset ();
		uint8_t available ();
};

void Data0 ();
void Data1 ();
extern uint32_t wiegandTimeout;
extern uint32_t pingNothification;

extern MyWiegand wiegand;

#endif


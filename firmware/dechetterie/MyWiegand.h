/**
 * \file MyWiegand.h
 * \author Guillaume R. <guillaume.rodrigues@etu.unistra.fr>
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

extern MyWiegand wiegand;

#endif /* _MYWIEGAND_h */

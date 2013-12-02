/**
 * \file MyUrl.h
 * \author Guillaume R. <guillaume.rodrigues@etu.unistra.fr>
 * \brief Handle url for Arduino. 
 */

#ifndef _MYURL_h
#define _MYURL_h

#include "Arduino.h"
#include "SdFat.h"
#include "SdBaseFile.h"

class MyUrl
{
	public:
		void init();
		uint8_t read();
		SdFat sd;
		byte ip [4];
		char url [256];
};

extern MyUrl url;

#endif /* _MYURL_h */


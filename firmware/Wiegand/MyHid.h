#ifndef _MYHID_h
#define _MYHID_h

#include "Arduino.h"

class MyHid
{
 private:
	byte speaker;
	byte led1;
	byte led2;
	void init_led ();
	void init_speaker ();

 public:
	void init();
	void bip (byte time);
	void blink_led1 (byte time);
	void blink_led2 (byte time);

};

extern MyHid hid;

#endif
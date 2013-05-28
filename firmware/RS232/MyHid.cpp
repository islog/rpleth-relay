#include "MyHid.h"

void MyHid::init()
{
	speaker = 12;
	led1 = 13;
	led2 = 11;
	init_led ();
	init_speaker ();
}

void MyHid::init_led ()
{
	pinMode(led1,OUTPUT);
	digitalWrite (led1, HIGH);
}

void MyHid::init_speaker ()
{
	pinMode(speaker,OUTPUT);
	digitalWrite (speaker, HIGH);
}

void MyHid::bip (byte time)
{
	digitalWrite (speaker, LOW);
	delay (time * 100);
	digitalWrite (speaker, HIGH);
}

void MyHid::blink_led1 (byte time)
{
	digitalWrite (led1, LOW);
	delay (time * 1000);
	digitalWrite (led1, HIGH);
}

void MyHid::blink_led2 (byte time)
{
	digitalWrite (led1, LOW);
	delay (time * 1000);
	digitalWrite (led1, HIGH);
}

MyHid hid;
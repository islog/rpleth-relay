#ifndef _MYRS_h
#define _MYRS_h

#include "Arduino.h"
#include "SoftwareSerial.h"

class MyRs
{
public:
	void send (byte * cmd, byte size);
	byte * receive (byte * size);
	void begin ();
private:
	uint8_t rxPin;
	uint8_t txPin;
	SoftwareSerial * com;
};

extern MyRs rs;

#endif

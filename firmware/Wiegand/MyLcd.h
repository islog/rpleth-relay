#ifndef _MYLCD_h
#define _MYLCD_h

#include "Arduino.h"
#include "LiquidCrystal.h"
#include "IPAddress.h"

class MyLcd
{
private:
	LiquidCrystal * lcd;
	uint8_t col;
	uint8_t ligne;
	uint8_t col_cur;
	uint8_t ligne_cur;
	uint8_t disp;
	uint8_t cursor;
	uint8_t first;
public:
	void begin(uint8_t col, uint8_t ligne, uint8_t rs, uint8_t enable, uint8_t d0, uint8_t d1, uint8_t d2, uint8_t d3);
	void begin(uint8_t col, uint8_t ligne, uint8_t rs, uint8_t rw, uint8_t enable, uint8_t d0, uint8_t d1, uint8_t d2, uint8_t d3);
	void clear ();
	void print (char c);
	void print (char * c);
	void print (const char * c);
	void print (byte * c,byte size);
	void print (IPAddress addr);
	void clearPrint (char c);
	void clearPrint (char * c);
	void clearPrint (const char * c);
	void setCursor (uint8_t col, uint8_t ligne);
	void display ();
	void blink ();
	void scroll ();
	uint8_t displayed ();
	byte is_blink;
	byte is_scroll;
	char * convert_to_char (int a);
};

extern MyLcd lcd;

#endif


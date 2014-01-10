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
  attachInterrupt (0, Data0, RISING);
  attachInterrupt (1, Data1, RISING);
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

MyWiegand wiegand;

void Data0()
{
  //Serial.print("0");
  wiegandTimeout = millis ();
  wiegand.bitCount ++;
  wiegand.bitHolder = wiegand.bitHolder << 1;
}

void Data1 ()
{
  //Serial.print("1");
  wiegandTimeout = millis ();
  wiegand.bitCount ++;
  wiegand.bitHolder = wiegand.bitHolder << 1;
  wiegand.bitHolder |= 1;
}


uint32_t wiegandTimeout = 0;
uint32_t pingNothification = 0;



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
  void bip (byte status);
  void setgreenled (byte status);
  void setredled (byte status);

};

extern MyHid hid;

#endif



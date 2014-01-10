#include "MyHid.h"

void MyHid::init()
{
  speaker = 7;
  led1 = 8;
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

void MyHid::bip (byte status)
{
  if (status)
    digitalWrite (speaker, LOW);
  else
    digitalWrite (speaker, HIGH);
}

void MyHid::setgreenled (byte status)
{
  if (status)
    digitalWrite (led1, LOW);
  else
    digitalWrite (led1, HIGH);
}

void MyHid::setredled (byte status)
{
  if (status)
    digitalWrite (led2, LOW);
  else
    digitalWrite (led2, HIGH);
}

MyHid hid;



#ifndef _MYCONST_h
#define _MYCONST_h

// Device
#define RPLETH 0x00
#define HID 0x01
#define LCD 0x02

// Rpleth Commands
#define STATEDHCP 0x01
#define DHCP 0x02
#define MAC 0x03
#define IP 0x04
#define SUBNET 0x05
#define GATEWAY 0x06
#define PORT 0x07
#define MESSAGE 0x08
#define RESET 0x09
#define PING 0x0A

// Hid Command
#define BEEP 0x00
#define GREENLED 0x01
#define REDLED 0x02
#define NOP 0x03
#define BADGE 0x04

// Lcd Command
#define DISPLAYS 0x00
#define DISPLAYT 0X01
#define BLINK 0X02
#define SCROLL 0X03
#define DISPLAYTIME 0x04

// Answer State
#define SUCCES 0x00
#define ECHEC 0x01
#define BADCHECKSUM 0x02
#define TIMEOUT 0x03
#define BADLENGHT 0x04
#define BADDEVICE 0x05

#endif



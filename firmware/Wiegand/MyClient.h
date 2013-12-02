#ifndef _MYCLIENT_H
#define _MYCLIENT_H

#include <Ethernet.h>
#include <string.h>

#define BUFFER_SIZE 128
#define HEADER_SIZE 3

class            MyClientManager
{
  public:
   MyClientManager() : checksum(0), checksumisset(false), bufferoffset(0), headeroffset(0) { memset(buffer,0,sizeof(buffer)); memset(header,0,sizeof(header)); }
   void reset() { checksum = 0; checksumisset = false; bufferoffset = 0; headeroffset = 0; memset(buffer,0,sizeof(buffer)); memset(header,0,sizeof(header)); };
   
   void read(EthernetClient client);
   bool haveCmd();
   bool check_checksum();
   
   byte            checksum;
   bool            checksumisset;
   char            bufferoffset;
   char            headeroffset;
   byte            header[HEADER_SIZE];  
   byte            buffer[BUFFER_SIZE]; 
};

#endif

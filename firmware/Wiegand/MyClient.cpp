#include "MyClient.h"

void MyClientManager::read(EthernetClient client)
{
  int data_size = client.available();
  int x = 0;
  Serial.println ("ENTER READ");
   Serial.println (data_size);
  if (headeroffset != 3 && data_size + headeroffset >= HEADER_SIZE)
  {
    while (headeroffset != HEADER_SIZE && x < data_size)
    {
      Serial.println ("read header");
      header[headeroffset] = client.read();
      ++headeroffset;
      ++x;
      //Serial.println ((int)headeroffset);
    }
    
    if (headeroffset == 3 && header[2] + 1 > BUFFER_SIZE)
      reset();
  }
  else if (headeroffset == 3 && bufferoffset != header[2] && data_size + bufferoffset >= header[2])
  {
    Serial.println ("DATA SIZE");
    //Serial.println ((int)(header[2]));
    while (bufferoffset != header[2] && x < data_size)
    {
      //Serial.println ("read buffer");
      buffer[bufferoffset] = client.read();
      ++bufferoffset;
      ++x;
      //Serial.println ((int)bufferoffset);
    }
  }
  else if (headeroffset == 3 && bufferoffset == header[2] && !checksumisset && data_size > 0)
  {
    Serial.println ("CHECKSUM GET");
    checksum = client.read();
    checksumisset = true;
  }
}

bool MyClientManager::haveCmd()
{
  return (headeroffset == 3 && bufferoffset == header[2] && checksumisset);
}

bool MyClientManager::check_checksum()
{
 // Serial.println("checksum");  
  
	byte result = 0, tmp = 0;
	for (int i = 0; i < 3; i++)
	{
		tmp ^= header[i];
	}
	for (int i = 0; i < header[2]; i++)
	{
		tmp ^= buffer [i];
	}
	if (tmp == checksum)
	{
		return true;
	}
	return false;
}

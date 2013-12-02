#include "MyUrl.h"

void MyUrl::init()
{
	ip[0] = 127;
	ip[1] = 0;
	ip[2] = 0;
	ip[3] = 1;
	url[0] = '\0';
}

uint8_t MyUrl::read()
{
	uint8_t result = 0;
	int j = 0;
	char tmp;
	if (sd.begin (4))
	{
		ifstream file("url.cfg");
		if (file.is_open()) 
		{
			for (int i = 0; i < 4; i++)
			{
				ip[i] = file.get();
			}
			j = 0;
			while ((tmp = file.get()) != EOF && tmp != '\0')
			{
				url[j] = tmp;
				j++;
			}
			url[j] = '\0';
			file.close();
			result = 1;
		}
	}  
	return result;
}


MyUrl url;


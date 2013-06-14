#include "MyRs.h"


void MyRs::begin ()
{
	rxPin = 2;
	txPin = 3;
	com = new SoftwareSerial (rxPin,txPin,true);
	com->begin(9600);
	pinMode(rxPin, INPUT);
	pinMode(txPin, OUTPUT);
	byte size;
	byte * ans = receive(&size);
	com->write ("v");
	free (ans);
	ans = receive(&size);
	free (ans);
}

void MyRs::send (byte * cmd, byte size)
{
	char * trame = (char *) malloc ((size + 1) * sizeof (char));
	for (int i = 0; i < size; i++)
	{
		trame [i] = cmd [i];
	}
	trame [size] = '\0';
	com->write(trame);
	free (trame);
}

byte * MyRs::receive (byte * size)
{
	// juste for wait the complete trame
	delay (200);
	int nb = com->available();
	byte * result = (byte *)malloc (nb * sizeof (byte));
	for (int i = 0; i < nb; i++)
	{
		result[i] = com->read();
	}
	*size = nb;
	return result;
}

MyRs rs;
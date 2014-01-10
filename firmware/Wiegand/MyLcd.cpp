#include "MyLcd.h"

void MyLcd::begin(uint8_t p_col, uint8_t p_ligne, uint8_t rs, uint8_t enable, uint8_t d0, uint8_t d1, uint8_t d2, uint8_t d3)
{
  lcd = new LiquidCrystal (rs, enable, d0, d1, d2, d3);
  lcd->begin (p_col, p_ligne);
  col = p_col;
  ligne = p_ligne;
  col_cur = 0;
  ligne_cur = 0;
  disp = 1;
  cursor = 0;
  first = 0;
  is_blink = 0;
  is_scroll = 0;
}

void MyLcd::begin(uint8_t p_col, uint8_t p_ligne, uint8_t rs, uint8_t rw, uint8_t enable, uint8_t d0, uint8_t d1, uint8_t d2, uint8_t d3)
{
  lcd = new LiquidCrystal (rs, rw, enable, d0, d1, d2, d3);
  lcd->begin (p_col, p_ligne);
  col = p_col;
  ligne = p_ligne;
  col_cur = 0;
  ligne_cur = 0;
  disp = 1;
  cursor = 0;
  first = 0;
  is_blink = 0;
  is_scroll = 0;
}


void MyLcd::print (char c)
{
  if (col_cur >= col)
  {
    if (ligne_cur +1 < ligne)
    {
      col_cur = 0;
      ligne_cur ++;
    }
    else
    {
      col_cur = 0;
      ligne_cur = 0;
      delay (4000);
      clear ();
    }
  }
  lcd->setCursor (col_cur, ligne_cur);
  col_cur ++;
  lcd->print (c);
}

void MyLcd::print (byte * c, byte size)
{
  for (int i = 0; i < size; i++)
  {
    print ((char)c[i]);
  }
}

void MyLcd::print (char * c)
{
  for (size_t i = 0; i < strlen (c); i++)
  {
    print (c[i]);
  }
}

void MyLcd::print (const char * c)
{
  for (size_t i = 0; i < strlen (c); i++)
  {
    print (c[i]);
  }
}

void MyLcd::print (IPAddress addr)
{
  for (int i = 0; i < 4; i++)
  {
    print ((char*)convert_to_char(addr[i]));
    if (i != 3)
      print ('.');
  }
}

void MyLcd::clear ()
{
  lcd->clear ();
  col_cur = 0;
  ligne_cur = 0;
}

void MyLcd::clearPrint (char c)
{
  clear ();
  print (c);
}

void MyLcd::clearPrint (char * c)
{
  clear ();
  print (c);
}

void MyLcd::clearPrint (const char * c)
{
  clear ();
  print (c);
}

void MyLcd::setCursor (uint8_t col, uint8_t ligne)
{
  lcd->setCursor (col, ligne);
}

void MyLcd::display ()
{
  lcd->display ();
  disp = 1;
}

void MyLcd::blink ()
{
  if (disp == 1)
  {
    lcd->noDisplay ();
    disp = 0;
  }
  else
  {
    lcd->display ();
    disp = 1;
  }
}

void MyLcd::scroll ()
{
  if (first == 0)
  {
    if (cursor != 15)
    {
      lcd->scrollDisplayLeft ();
      cursor ++;
    }
    else
    {
      for (int i = 0; i < 10; i++)
      {
        lcd->scrollDisplayLeft ();
      }
      cursor = 0;
      first = 1;
    }
  }
  else
  {
    if (cursor != 31)
    {
      lcd->scrollDisplayLeft ();
      cursor ++;
    }
    else
    {
      for (int i = 0; i < 9; i++)
      {
        lcd->scrollDisplayLeft ();
      }
      cursor = 0;
    }
  }
}

uint8_t MyLcd::displayed ()
{
  return disp;
}

char * MyLcd::convert_to_char (int a)
{
  char * result;
  if (a < 10)
  {
    result = (char *)malloc (2 * sizeof (char));
    result[0] = a + 48;
    result[1] = '\0';
  }
  else if (a < 100)
  {
    result = (char *)malloc (3 * sizeof (char));
    result[0] = a/10 + 48;
    result[1] = a%10 + 48;
    result[2] = '\0';
  }
  else
  {
    result = (char *)malloc (4 * sizeof (char));
    result[0] = (a/100)%10 + 48;
    result[1] = (a/10)%10 + 48;
    result[2] = a%10 + 48;
    result[3] = '\0';
  }
  return result;
}

MyLcd lcd;




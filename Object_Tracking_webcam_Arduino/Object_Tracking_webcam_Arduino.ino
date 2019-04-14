#include <Servo.h>   // Servo motor kütüphanesi tanımlama
Servo servo_sagsol;  //Servo motor değişken isimleri
Servo servo_altust;  //Servo motor değişken isimleri

int pos1 = 90;       //Servo motor Açı değişkenleri
int pos2 = 90;       //Servo motor Açı değişkenleri
int gelenVeri;       //C# tan gelen bilgilerin tutulduğu değişken
 
void setup()   // 1 kereye mahsus çalışır
{
  Serial.begin(9600);        //C# ile haberleşme hızı
  servo_sagsol.attach(6);    //Servo motorların Arduino'da hangi pine bağlı olduğunu tanımlama
  servo_altust.attach(7);    //Servo motorların Arduino'da hangi pine bağlı olduğunu tanımlama
  servo_sagsol.write(pos1);  //Başlangıçtaki servo açılarını 90 derece olarak ayarlama
  servo_altust.write(pos2);  //Başlangıçtaki servo açılarını 90 derece olarak ayarlama

}

void loop() 
{
    if(Serial.available())   // Serial haberleşme var ise,
  {
    gelenVeri=Serial.read();  //Serial haberleşmeden gelen veriyi oku değişkene ata

    switch(gelenVeri)        //gelen veri var ise içeride kal
    {
      case '1': //pos1+ sola, pos2+ üste   (pos1=sağ ve sol motoru açı değişkeni, pos2 üste ve alta motor açı değişkeni)
       pos1+=5;
       pos2+=5;
       delay(5);
       if (pos1 > 180)
       {
         pos1=180;
       }
       if(pos2>180)
       {
        pos2=180;
       }
      break;

      case '2': // pos2+ üste
       pos2+=5;
       delay(5);
       if(pos2>180)
       {
        pos2=180;
       }       
      break;  

      case '3': // pos1- saga, pos2+ üste
        pos1-=5;
       delay(5);
       if(pos1<0)
        {
          pos1=0;
        } 
      break;

      case '4': // pos1+ sola, pos2- alta
       pos1+=5;
       pos2-=5;
       delay(5);
       if (pos1 > 180)
       {
         pos1=180;
       } 
       if(pos2<0)
       {
        pos2=0;     
       }
      break;

      case '5':  // pos2- alta
       pos2-=5;
       delay(5);
       if(pos2<0)
       {
        pos2=0;
       }

      break;

      case '6': // pos1- saga, pos2- alta 
       pos1-=5;
       pos2-=5;
       delay(5);
       if(pos1<0)
        {
          pos1=0;
        } 
       if(pos2<0)
       {
        pos2=0; 
       }
            
    }
   servo_sagsol.write(pos1);
   servo_altust.write(pos2);
  }
  

}

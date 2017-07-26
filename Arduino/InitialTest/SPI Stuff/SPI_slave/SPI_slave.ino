#include <SPI.h>
#define SS 10
#define MOSI 11
#define MISO 12
#define SCK 13

byte SPItransfer(byte value) {
  SPDR = value;
  while(!(SPSR & (1<<SPIF)));
  delay(10);
  return SPDR;
}

void setup() {
  Serial.begin(9600);
  SPCR = (1 << SPE);
  pinMode(MISO,INPUT);
  pinMode(SCK,INPUT);
  pinMode(MOSI,INPUT);
  pinMode(SS,INPUT);
}

void loop() {
  if (digitalRead(SS)==LOW){
    pinMode(MISO,OUTPUT);
    //Serial.println("selected");
    byte rx = SPItransfer(111);
    
  }
  else{
    Serial.println("not");
  }

}

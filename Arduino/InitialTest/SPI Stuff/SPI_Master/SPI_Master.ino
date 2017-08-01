#include <SPI.h>
#define SS 10
#define MOSI 11
#define MISO 12
#define SCK 13

void setup() {
  Serial.begin(9600);
  digitalWrite(SS, HIGH); //slave pin stays at high voltage until data comes
  pinMode(SCK, OUTPUT);
  pinMode(MOSI, OUTPUT);
  pinMode(SS, OUTPUT);
  digitalWrite(MOSI, LOW);
  digitalWrite(SCK, LOW);
  SPI.begin();
}

void loop() {
  digitalWrite(SS, LOW); //enable slave select
  Serial.println("selected");
  delay(20);
  byte rx = SPI.transfer(255);
  Serial.println(rx);
  digitalWrite(SS, HIGH);
  delay(5000);

}

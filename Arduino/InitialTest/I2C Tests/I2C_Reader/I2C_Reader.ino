#include <Wire.h>

void setup() {
  Wire.begin();        // join i2c bus (address optional for master)
  Serial.begin(9600);  // start serial for output
}

void loop() {
  Wire.requestFrom(8, 1);    // request 1 byte from slave device #8

  while (Wire.available()) { // slave may send less than requested
    double c = Wire.read(); // receive a byte as character
    Serial.println(c);         // print the character
  }
  Wire.requestFrom(9, 1);    // request 1 byte from slave device #9

  /*while (Wire.available()) { // slave may send less than requested
    double c = Wire.read(); // receive a byte as character
    Serial.println(c);         // print the character
  }*/
  delay(500);
}

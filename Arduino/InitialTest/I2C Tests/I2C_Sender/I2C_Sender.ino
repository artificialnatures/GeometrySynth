#include <Wire.h>
int potPin = A2;
int potVal = 0;
void setup() {
  Wire.begin(8);                // join i2c bus with address #8
  Wire.onRequest(requestEvent); // register event
}

void loop() {
  delay(100);
}

// function that executes whenever data is requested by master
// this function is registered as an event, see setup()
void requestEvent() {
   potVal = analogRead(potPin);
   
  Wire.write(potVal/4); // can only send values 0-255, so divide by 4
}

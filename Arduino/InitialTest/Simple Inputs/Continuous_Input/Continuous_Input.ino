/*
Reads input from a knob/slider
*/
int potPin = A2;

//If there are multiple inputs to read
int potPin2 = A0;
int potPin3 = A1;

int potVal = 0;
int potVal2 = 0;
int potVal3 = 0;

int time = 100; //time between readings (ms)
void setup() {
Serial.begin(9600);
}

void loop() {
potVal = analogRead(potPin);
potVal2 = analogRead(potPin2);
potVal3 = analogRead(potPin3);

Serial.print("Sliding Pot: ");
Serial.println(potVal/4);
//Serial.print("Unnumbered Pot: ");
//Serial.println(potVal2);
//Serial.print("Numbered Pot: ");
//Serial.println(potVal3);
//float finalVal = (float) potVal/1024.0; // scales between 0-1
//Serial.println(finalVal);
delay(time);
}

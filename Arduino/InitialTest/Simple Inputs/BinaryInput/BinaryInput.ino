/* 
Reads input from momentary/toggle switches. Connect to C and NC, so that pressed = 1, open = 0
*/
int buttonPin = 2;
void setup(){
  pinMode(buttonPin, INPUT);
  digitalWrite(buttonPin,HIGH);
  Serial.begin(9600);
}

void loop(){
  int reading = digitalRead(buttonPin);
  Serial.println(reading);
  delay(100);
}

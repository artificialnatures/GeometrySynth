//echo replies with the same message it receives
String message = "";
char EOL = '\n';

void setup() 
{
  Serial.begin(9600);
}

void loop() 
{
  
}

void serialEvent()
{
  message = Serial.readStringUntil(EOL);
  Serial.write(message.c_str());
}


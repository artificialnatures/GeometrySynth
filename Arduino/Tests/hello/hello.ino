int send_interval = 100;
String message = "{\"message\":\"hello\"}";

void setup() 
{
  Serial.begin(19200);
}

void loop() 
{
  Serial.println(message);
  delay(send_interval);
}

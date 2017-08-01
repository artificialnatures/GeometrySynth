void setup() {
  Serial.begin(9600);
}

void loop() {
  int incoming = Serial.available();

 if (incoming>0){
  Serial.print(incoming);
 }

}

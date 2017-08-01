float val;
float samples = 1000;
float count = 0;
int iteration = 0;

void setup() {
  Serial.begin(9600);
}

void loop() {
  /*if (iteration < samples) {
    if (analogRead(A0) > 0) {
      count++;
    }
    iteration++;
  }
  else {
    val = count / samples;
    float final = val * 255;
    Serial.println(final);
    iteration = 0;
    count = 0;
  }*/
  float duration = pulseIn(A0,HIGH);
  float value = (duration/2020)*255;
  Serial.println(value);
  delay(500);
}

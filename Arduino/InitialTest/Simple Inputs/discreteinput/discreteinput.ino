/*
Reads discrete inputs from a 10-position rotary switch, using 16.5 kOhm resistors
*/
int potPin = A2;
int potVal = 0;
float resistorValue = 16500; //using 16.5kOhm resistors
int numpositions = 10; //we are using a 10-position rotary switch
int divisor = numpositions - 1; //the voltage for the first and last positions will be 0 & 1024, so the voltage will increase incrementally across each position in between
int bincenter = 1024/divisor;
int bufferZone = 25; //values vary, so we'll look in a zone of +/-25
int time = 1000; //time between readings (ms)
void setup() {
Serial.begin(9600);
}

void loop() {
  potVal = analogRead(potPin);
  for(int i=0;i<10;i++){
    if(potVal>(bincenter*i-bufferZone) && potVal<(bincenter*i+bufferZone)){
      potVal=i+1;
    }
  }
  Serial.println(potVal);
}

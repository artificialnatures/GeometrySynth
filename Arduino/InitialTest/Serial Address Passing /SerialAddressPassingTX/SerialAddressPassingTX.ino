#define ADDRESS 0
int rx = 0;
int tx=1;
int numModules = 2;
int addresses[2];
void setup() {
  Serial.begin(9600);
  pinMode(rx,INPUT);
  pinMode(tx,OUTPUT);
}

void loop() {
  Serial.println(ADDRESS);
  delay(1000);

}

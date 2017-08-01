/* sketch to pass the address when two modules are patched together*/
#define ADDRESS 1
int rx = 0;
int tx = 1;
int numModules = 2;
int addresses[5]; //where 2=numModules
String Addresses;
byte bytearray[64];
void setup() {
  Serial.begin(9600);
  pinMode(rx,INPUT);
  pinMode(tx,OUTPUT);
}

void loop() {
  int incoming = Serial.available()/sizeof(int);
  //while (incoming>0){ //only happens if module has something to recieve
  if (incoming>0){
    for (int i=0;i<incoming;i=i+1){
      addresses[i] = Serial.parseInt();
      Serial.println(addresses[i]);
      //Serial.println(ADDRESS);
    }
    Serial.println(ADDRESS);
    Serial.readBytes(bytearray,Serial.available());
   // Serial.println(incoming);
   // delay(1000);
   // incoming = 0;
 //   incoming = Serial.available(); //make sure something is still patched in
  }
}

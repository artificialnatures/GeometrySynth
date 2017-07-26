#define ADDRESS 2
int rx = 0;
int tx=1;
int numModules = 2;
int addresses[5];
String Addresses;
void setup() {
  Serial.begin(9600);
  pinMode(rx,INPUT);
  pinMode(tx,OUTPUT);

}

void loop() {
  
  int incoming = Serial.available()/sizeof(int);
  //Serial.println(incoming);
  
  //while (incoming>0){ //only happens if module has something to recieve
  if (incoming>0){
    for (int i=0;i<incoming;i=i+1){
      //Serial.println("in the loop");
      addresses[i] = Serial.parseInt();
      Serial.print(addresses[i]);
      //Serial.println(ADDRESS);
    }
    //Serial.println("left the loop");
    //Serial.println(ADDRESS);
   //incoming = Serial.available(); //check to see if still plugged in
  }
}

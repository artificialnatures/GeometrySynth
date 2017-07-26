#include <Wire.h>

#define NODE_ADDRESS 3  // Change this unique address for each I2C slave node
#define PAYLOAD_SIZE 2 // Number of bytes  expected to be received by the master I2C node
byte nodePayload[PAYLOAD_SIZE];

int rx = 0;
int tx = 1;
int addresses[5]; //where 2=numModules
byte bytearray[64];

void setup()
{

  Serial.begin(9600);  
  Serial.println("SLAVE SENDER NODE");
  Serial.print("Node address: ");
  Serial.println(NODE_ADDRESS);
  Serial.print("Payload size: ");
  Serial.println(PAYLOAD_SIZE);
  pinMode(2, INPUT);
  digitalWrite(2,HIGH);
  Serial.println("***********************");

  Wire.begin(NODE_ADDRESS);  // Activate I2C network
  Wire.onRequest(requestEvent); // Request attention of master node

  pinMode(rx,INPUT);
  pinMode(tx,OUTPUT);
}

void loop()
{ 
  //delay(100);
  nodePayload[0] = NODE_ADDRESS; // I am sending Node address back.  Replace with any other data 
  nodePayload[1] = digitalRead(2); 

  int incoming = Serial.available()/sizeof(int);
  
  if (incoming>0){
    for (int i=0;i<incoming;i=i+1){
      addresses[i] = Serial.parseInt();
      Serial.println(addresses[i]);
    }
    Serial.println(NODE_ADDRESS);
    Serial.readBytes(bytearray,Serial.available());
  }
}

void requestEvent()
{
  Wire.write(nodePayload,PAYLOAD_SIZE);  
  //Serial.print("Sensor value: ");  // for debugging purposes. 
  //Serial.println(nodePayload[1]); // for debugging purposes. 
}

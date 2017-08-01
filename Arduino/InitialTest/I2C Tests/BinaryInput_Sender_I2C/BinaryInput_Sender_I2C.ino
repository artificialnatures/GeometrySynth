#include <Wire.h>

#define NODE_ADDRESS 2  // Change this unique address for each I2C slave node
#define PAYLOAD_SIZE 2 // Number of bytes  expected to be received by the master I2C node
byte nodePayload[PAYLOAD_SIZE];

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
}

void loop()
{ 
  delay(100);
  nodePayload[0] = NODE_ADDRESS; // I am sending Node address back.  Replace with any other data 
  nodePayload[1] = digitalRead(2); 
}

void requestEvent()
{
  Wire.write(nodePayload,PAYLOAD_SIZE);  
  Serial.print("Sensor value: ");  // for debugging purposes. 
  Serial.println(nodePayload[1]); // for debugging purposes. 
}

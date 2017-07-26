//Send 3 inputs + address to master from a single arduino

#include <Wire.h>

#define NODE_ADDRESS 1  // Change this unique address for each I2C slave node
#define PAYLOAD_SIZE 5 // Number of bytes  expected to be received by the master I2C node

byte nodePayload[PAYLOAD_SIZE];

void setup()
{

  Serial.begin(9600);
  Serial.println("SLAVE SENDER NODE");
  Serial.print("Node address: ");
  Serial.println(NODE_ADDRESS);
  Serial.print("Payload size: ");
  Serial.println(PAYLOAD_SIZE);
  Serial.println("***********************");

  Wire.begin(NODE_ADDRESS);  // Activate I2C network
  Wire.onRequest(requestEvent); // Request attention of master node
}

void loop()
{
  nodePayload[0] = PAYLOAD_SIZE;
  nodePayload[1] = NODE_ADDRESS;
  nodePayload[2] = analogRead(A2) / 4; // Read A0 and fit into 1 byte. Replace this line with your sensor value
  nodePayload[3] = analogRead(A0)/4;
  nodePayload[4] = analogRead(A1) / 4;
  delay(100);
}

void requestEvent()
{
  //Wire.write(PAYLOAD_SIZE,1);
  Wire.write(nodePayload, PAYLOAD_SIZE);
  //Serial.print("Sensor value: ");  // for debugging purposes.
  //Serial.println(nodePayload[1]); // for debugging purposes.
}

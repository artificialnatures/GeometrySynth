/*
  Script for the first 'create'/transmitter Arduino.  Current code written for a 10-pos rotary switch to create initial geometry.  Change PAYLOAD_SIZE if necessary
  Adapted from bizar
*/
#include <Wire.h>

#define NODE_ADDRESS 1  // Change this unique address for each I2C slave node
#define PAYLOAD_SIZE 4 // Number of bytes  expected to be received by the master I2C node

byte nodePayload[PAYLOAD_SIZE];

int rx = 0; //serial rx
int tx = 1; //serial tx
int potPin = A2;
int potVal = 0;
float resistorValue = 16500; //using 16.5kOhm resistors
int numpositions = 10; //we are using a 10-position rotary switch
int divisor = numpositions - 1; //the voltage for the first and last positions will be 0 & 1024, so the voltage will increase incrementally across each position in between
int bincenter = 1024 / divisor;
int bufferZone = 15; //values vary, so we'll look in a zone of +/- 15
int time = 1000; //time between readings (ms)

boolean needsUpdate = false;
int currentVal;
int passedVal;
void setup()
{

  Serial.begin(115200);
  Serial.println("SLAVE SENDER NODE");
  Serial.print("Node address: ");
  Serial.println(NODE_ADDRESS);
  Serial.print("Payload size: ");
  Serial.println(PAYLOAD_SIZE);
  Serial.println("***********************");

  Wire.begin(NODE_ADDRESS);  // Activate I2C network
  Wire.onRequest(requestEvent); // Request attention of master node

  pinMode(rx, INPUT);
  pinMode(tx, OUTPUT);

  //analogWrite(3, 5); //passing address- the address for this module will be 5
}

void loop()
{
 // analogWrite(3, 254); //passing address- the address for this module will be 5
  Serial.println(100);
  
  //only send new data packet if the input value has been changed
  int readVal = analogRead(potPin);
  if (readVal < currentVal - 20 || readVal > currentVal + 20) { //think more about this range
    currentVal = readVal;
    needsUpdate = true;
  }
  else {
    needsUpdate = false;
  }
  nodePayload[0] = needsUpdate; //this will be the first value sent to the master

  if (needsUpdate) {
    //Serial.println("update"); //for debugging
    nodePayload[1] = PAYLOAD_SIZE; //send size of payload
    nodePayload[2] = NODE_ADDRESS; //send the node address
    //gives values 1-10 for rotary switch based on resistance ladder
    
    for (int i = 0; i < 10; i++) {
      if (currentVal > (bincenter * i - bufferZone) && currentVal < (bincenter * i + bufferZone)) {
        passedVal = i + 1;
      }
    }
    
    nodePayload[3] = passedVal; //send rotary switch value
    needsUpdate = false;
  }

  //analogWrite(3, 5); //passing address- the address for this module will be 5
  delay(100); //reduce to lowest possible time

}

void requestEvent()
{
  Wire.write(nodePayload, PAYLOAD_SIZE); //send the packetized data upon request of the master
  //Serial.print("Sensor value: ");  // for debugging
  //Serial.println(nodePayload[1]); // for debugging
}

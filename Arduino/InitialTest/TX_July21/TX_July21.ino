/*
  Script for the first 'create'/transmitter Arduino.  Current code written for a 10-position rotary switch to create initial geometry.  Change PAYLOAD_SIZE if necessary
  I2C communication adapted from techbizar.com
*/
#include <Wire.h>
#include <SoftwareSerial.h>  //library to turn pair of digital pins into serial rx/tx

#define NODE_ADDRESS 1  // Change this unique address for each I2C node
#define PAYLOAD_SIZE 5 // Number of bytes  expected to be received by the master I2C node

byte nodePayload[PAYLOAD_SIZE];

int rx = 0; //serial rx
int tx = 1; //serial tx
int potPin = A2;
int potVal = 0;
float resistorValue = 16500; //using 16.5kOhm resistors
int numpositions = 10; //we are using a 10-position rotary switch
int divisor = numpositions - 1; //the voltage for the first and last positions will be 0 & 1024, so the voltage will increase incrementally across each position in between
int bincenter = 1024 / divisor;
int bufferZone = 20; //values vary, so we'll look in a zone of +/- 20
int time = 1000; //time between readings (ms)

boolean needsUpdate = false;  //boolean to send payload to master upon changes
int currentVal; //constantly reading input values
int passedVal; //value being passed to master 

int incomingAddress; //reads the address of any module connected to this one
int cashedAddress; //stores the address of module connected to this one



SoftwareSerial portOne(10, 11); // (RX, TX)
//SoftwareSerial portTwo(8,9);  //add if additional serial communication is neccessary.  SoftSerial library doesn't allow simultaneous data transfer, so incompatible with current code 
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

  portOne.begin(9600);
  //portTwo.begin(9600);
  pinMode(rx, INPUT);
  pinMode(tx, OUTPUT);
}

void loop()
{
  //constantly sending out node address via portOne
  portOne.println(NODE_ADDRESS);
  
  //only send new data payload if the input value has been changed
  int readVal = analogRead(potPin);
  if (readVal < currentVal - bufferZone || readVal > currentVal + bufferZone) { //think more about this range
    currentVal = readVal;
    needsUpdate = true;
  }
  else {
    needsUpdate = false;
  }

  //gives values 1-10 for rotary switch based on resistance ladder
    for (int i = 0; i < 10; i++) {
      if (currentVal > (bincenter * i - bufferZone) && currentVal < (bincenter * i + bufferZone)) {
        passedVal = i + 1;
      }
    }

  //Update Address
  int incoming = portOne.available(); //check if any module is being piped into this one
  if (incoming > 0) {
    incomingAddress = portOne.parseInt();
  }
  else {
    incomingAddress = 0;
  }

  if (cashedAddress != incomingAddress) {
    needsUpdate = true;
    cashedAddress = incomingAddress;
  }
  //Send data to master
  nodePayload[0] = needsUpdate; //this will be the first value sent to the master. master will only request data if needsUpdate is true
  if (needsUpdate) {
    nodePayload[1] = PAYLOAD_SIZE; //send size of payload
    nodePayload[2] = NODE_ADDRESS; //send the node address
    nodePayload[3] = passedVal; //send rotary switch value
    nodePayload[4] = cashedAddress;
    needsUpdate = false;
  }
  //delay(100); //reduce to lowest possible time

}

void requestEvent()
{
  Wire.write(nodePayload, PAYLOAD_SIZE); //send the packetized data upon request of the master
  //Serial.print("Sensor value: ");  // for debugging
  //Serial.println(nodePayload[1]); // for debugging
}

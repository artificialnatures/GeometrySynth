/*
  Script for the final/Master Arduino in the chain.  Change NODE_MAX based on how many Arduinos are being used.
  Need to think about how big to make PAYLOAD_SIZE and addresses[].
   I2C communication adapted from techbizar.com
*/
#include <Wire.h>

#define PAYLOAD_SIZE 16 // how many bytes to expect from each node.  should probably be max number from any node
int NODE_MAX = 4; // maximum number of slave nodes (I2C addresses) to probe, change accordingly
#define START_NODE 1 // The starting I2C address of slave nodes
#define NODE_READ_DELAY 100 // Some delay between I2C node reads

int nodePayload[PAYLOAD_SIZE]; //for reading in I2C data
int rx = 0; //serial reciever
int tx = 1; // serial transmitter

float val;
float samples = 1000;
float cashedAddress = 0;
float count = 0;
int iteration = 0;

float addressRecieved;

int counter = 0;

void setup()
{
  Serial.begin(115200);
  Serial.println("MASTER READER NODE");
  Serial.print("Maximum Slave Nodes: ");
  Serial.println(NODE_MAX);
  Serial.print("Payload size: ");
  Serial.println(PAYLOAD_SIZE);
  Serial.println("***********************");

  Wire.begin();        // Activate I2C link

  pinMode(rx, INPUT);
  pinMode(tx, OUTPUT);

}

void loop()
{
  //retrieve and print I2C data
  for (int nodeAddress = START_NODE; nodeAddress <= NODE_MAX; nodeAddress++) { // we are starting from Node address 1
    Wire.requestFrom(nodeAddress, PAYLOAD_SIZE); //the first value being sent is needsUpdate, to check if anything has changed.  The second is the payload size, which is used if update is needed
    int bytesToRead = Wire.available();
    //Serial.println(bytesToRead);
    /*
    boolean needsUpdate = Wire.read();
    //Serial.println(needsUpdate);
    */
    //if (needsUpdate) {
      //int Payload = Wire.read(); //^
      //Wire.requestFrom(nodeAddress, Payload);    //now we'll request all of the data
     // if (Wire.available()) { // if data size is avaliable from nodes
        for (int i = 0; i < bytesToRead; i++) nodePayload[i] = Wire.read();  // get nodes data
        for (int i = 0; i < 7; i++) Serial.println(nodePayload[i]);   // start at j=3, since we don't need to print needsUpdate & size of payload
      }
   // }
  //}
  //delay(NODE_READ_DELAY); //reduce to lowest possible time(?)
}



/*
  Script for the final/Master Arduino in the chain.  Change NODE_MAX based on how many Arduinos are being used.
  Need to think about how big to make PAYLOAD_SIZE and addresses[].
*/
#include <Wire.h>

#define PAYLOAD_SIZE 6 // how many bytes to expect from each node.  should probably = 6, if the most inputs on any module will be 3, plus needsUpdate, size of payload, and address
#define NODE_MAX 4 // maximum number of slave nodes (I2C addresses) to probe, change accordingly
#define START_NODE 1 // The starting I2C address of slave nodes
#define NODE_READ_DELAY 100 // Some delay between I2C node reads

int nodePayload[PAYLOAD_SIZE]; //for reading in I2C data
int addresses[5]; //to keep track of addresses & order.  size should be # of possible arduinos in chain (?)
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

  //address recieving
  /*
    float duration = pulseIn(A0, HIGH);
    float value = (duration / 2100.0) * 255;
    Serial.println(value);
    if (value < cashedAddress - 2 || value > cashedAddress + 2) {
    cashedAddress = value;
    Serial.println(cashedAddress);
    }
  */
  //Serial.println(duration);
  /*
    if (iteration < samples) {
    if (analogRead(A0) > 0) {
      count++;

    }
    iteration++;
    //Serial.println(iteration);
    }
    else {
    val = count / samples;
    float finalVal = val * 255;
    //float mappedVal = map(finalVal,0,255,0,1023);
    if(finalVal>200){
      //disconnected
    }
    else if (finalVal > addressRecieved + 3 || finalVal < addressRecieved - 3) {
      addressRecieved = finalVal;
      Serial.println(finalVal);
    }
    iteration = 0;
    count = 0;
    }
  */

  //first part of loop retrieves and prints the order in which the arduinos are patched together
  /*
    int incoming = Serial.available() / sizeof(int);
    if (incoming > 0) { //if there is something plugged into the master
    int chain = Serial.parseInt(); //the first piece of data sent from relay nodes counts the length of the chain
    Serial.println("Modules Connected:");
    for (int i = 0; i < chain; i = i + 1) {
      addresses[i] = Serial.parseInt();
      Serial.println(addresses[i]); //retrieve and print address data
    }
    }
  */
  //now retrieve and print I2C data

  for (int nodeAddress = START_NODE; nodeAddress <= NODE_MAX; nodeAddress++) { // we are starting from Node address 1
    Wire.requestFrom(nodeAddress, 2); //the first value being sent is needsUpdate, to check if anything has changed.  The second is the payload size, which is used if update is needed
    boolean needsUpdate = Wire.read();
    //Serial.println(needsUpdate);
    if (needsUpdate) {
      int Payload = Wire.read(); //^
      Wire.requestFrom(nodeAddress, Payload);    //now we'll request all of the data
      if (Wire.available() == Payload) { // if data size is avaliable from nodes
        for (int i = 0; i < Payload; i++) nodePayload[i] = Wire.read();  // get nodes data
        for (int j = 3; j < Payload; j++) Serial.println(nodePayload[j]);   // start at j=3, since we don't need to print needsUpdate & size of payload
        Serial.println("*************************");
      }
    }
  }


  //As opposed to the above block of code, this chunk only prints the data out if the module is connected
  /*for (int nodeAddress = START_NODE; nodeAddress <= NODE_MAX; nodeAddress++) { // we are starting from Node address 1
    for (int checker = 0; checker < NODE_MAX; checker++) { //compare all addresses to addresses in chain
      if (nodeAddress == addresses[checker]) {
        Wire.requestFrom(nodeAddress, 1); //The first piece of data being sent is the # of pieces of data.  used to loop through correct number
        int Payload = Wire.read(); //^
        Wire.requestFrom(nodeAddress, Payload);    //now we'll request the rest of the data
        if (Wire.available() == Payload) { // if data size is avaliable from nodes
          for (int i = 0; i < Payload; i++) nodePayload[i] = Wire.read();  // get nodes data
          for (int j = 0; j < Payload; j++) Serial.println(nodePayload[j]);   // print nodes data
          Serial.println("*************************");
        }
      }
    }
    }*/
  memset(addresses, 0, sizeof(addresses));
  //delay(NODE_READ_DELAY); //reduce to lowest possible time(?)
}



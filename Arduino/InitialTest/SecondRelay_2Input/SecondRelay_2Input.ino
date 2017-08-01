/*
  Sketch for each intermediate/relay module.  Change NODE_ADDRESS so each module has a distinct address, and PAYLOAD_SIZE depending on how many inputs there are
*/
#include <Wire.h>
#include <SoftwareSerial.h>
#define NODE_ADDRESS 3  // Change this so each module has unique addresss
#define PAYLOAD_SIZE 6 // Number of bytes  expected to be received by the master I2C node.  Change as needed
byte nodePayload[PAYLOAD_SIZE]; //data packet to be sent to master

int rx = 0; //serial rx
int tx = 1; //serial tx
byte bytearray[64]; //used to clear incoming data from serial port
boolean needsUpdate = false;
int currentVal;
int currentVal2;
int currentVal3;
int cashedAddress = 0;
int cashedAddress2 = 0;
int incomingAddress = 0;
int incomingAddress2 = 0;


SoftwareSerial portOne(10, 11); // RX, TX
//SoftwareSerial portTwo(8, 9);
void setup()
{

  Serial.begin(115200);
  Serial.println("SLAVE SENDER NODE");
  Serial.print("Node address: ");
  Serial.println(NODE_ADDRESS);
  Serial.print("Payload size: ");
  Serial.println(PAYLOAD_SIZE);
  pinMode(2, INPUT); //for buttons
  digitalWrite(2, HIGH); //^
  Serial.println("***********************");

  Wire.begin(NODE_ADDRESS);  // Activate I2C network
  Wire.onRequest(requestEvent); // Request attention of master node

  portOne.begin(9600);
//  portTwo.begin(9600);
  pinMode(rx, INPUT);
  pinMode(tx, OUTPUT);
}

void loop()
{
  portOne.println(NODE_ADDRESS);
  int readVal = analogRead(A2) / 4;
  int readVal2 = analogRead(A1) / 4;
  int readVal3 = analogRead(A0) / 4;
  
  if (readVal < currentVal - 5 || readVal > currentVal + 5) {
    //Serial.println("updating");
    currentVal = readVal;
    //Serial.println(currentVal);
    needsUpdate = true;
  }
  
  if (readVal2 < currentVal2 - 5 || readVal2 > currentVal2 + 5) {
    //Serial.println("updating");
    currentVal2 = readVal2;
    needsUpdate = true;
  }
  /*
  if (readVal3 < currentVal3 - 5 || readVal3 > currentVal3 + 5) {
    //Serial.println("updating");
    currentVal3 = readVal3;
    needsUpdate = true;
  }
  */
    int incoming = portOne.available(); //check if any module is being piped into this one (do i need to divide by sizeof(int)?)
    //Serial.println(incoming);
    if (incoming > 0) {
      incomingAddress = portOne.parseInt();
      //Serial.println(incomingAddress);
    }
    else {
      incomingAddress = 0;
    }
  


  if (cashedAddress != incomingAddress) {
    needsUpdate = true;
    cashedAddress = incomingAddress;
  }

  if (cashedAddress2 != incomingAddress2) {
    needsUpdate = true;
    cashedAddress2 = incomingAddress2;
  }


  nodePayload[0] = needsUpdate;
  if (needsUpdate) {
    nodePayload[1] = PAYLOAD_SIZE; //first item is size of payload
    nodePayload[2] = NODE_ADDRESS; //send the address
    nodePayload[3] = currentVal; //send first input data
    nodePayload[4] = currentVal2;
    nodePayload[5] = cashedAddress;
    //nodePayload[4] = currentVal2;
    //nodePayload[5] = currentVal3;
    //nodePayload[6] = cashedAddress;
  }
  //Serial.readBytes(bytearray, Serial.available()); //this reads the incoming data from the serial port, effectively clearing it, so we're ready to loop again
  //portOne.read();
//  portTwo.read();
  needsUpdate = false;
 }

void requestEvent()
{
  Wire.write(nodePayload, PAYLOAD_SIZE); //send data upon request of master
  //Serial.print("Sensor value: ");  // for debugging purposes.
  //Serial.println(nodePayload[1]); // for debugging purposes.
}



/* Constants, local
 * These values are not shared with the Unity project
 */
enum ModuleState
{
  SEARCHING,
  CONNECTED,
  UPDATING,
  PAUSED,
  EXITING
};
const int TOLERANCE = 5;
/* Constants, shared
 * Ensure these values match those in the Unity project (GeometrySynth.Constants)
 */
enum ReservedAddresses
{
    EMPTY = 0,
    HUB = 1
};
enum ModuleFunction
{
    PASSTHROUGH = 2,
    //Creation:
    SHAPE = 3,
    //Transformation:
    TRANSLATE = 10,
    ROTATE = 11,
    SCALE = 12,
    MATRIX = 13,
    //Multipliers:
    WAVE = 20,
    SINE_WAVE = 21,
    SQUARE_WAVE = 22,
    TRIANGLE_WAVE = 23,
    PULSE = 24,
    NOISE = 25,
    NOISE_PERLIN = 26,
    //Material:
    COLOR = 30,
    COLOR_ALPHA = 31,
    TEXTURE = 32,
    BUMP = 33,
    //Lights:
    LIGHT = 40,
    LIGHT_DIRECTIONAL = 41,
    LIGHT_SPOT = 42,
    LIGHT_POINT = 43,
    //Camera:
    CAMERA_POSITION = 50,
    CAMERA_ORBIT = 51,
    CAMERA_PAN = 52,
    CAMERA_FOV = 53,
    //Filters:
    FILTER_LOWPASS = 60,
    FILTER_HIGHPASS = 61,
    //Triggers:
    TOGGLE = 70,
    BLINK = 71
};
enum Command
{
    CONNECT = 1,
    DISCONNECT = 2,
    CREATE = 3,
    READ = 4,
    UPDATE = 5,
    DELETE = 6,
    PING = 7,
    HELLO = 8,
    LINK = 9,
    UNLINK = 10
};
//End of shared constants
//Module Definition:
int address = 10;
const ModuleFunction moduleFunction = SINE_WAVE;
ModuleState moduleState = SEARCHING;
const int inputCount = 3; //number of inputs, from 0 to 6, inputs are mapped to analog pins
int inputPins[inputCount];
int inputValues[inputCount];
boolean requiresUpdate = false;

void setup() 
{
  Serial.begin(9600);
  for (int i = 0; i < inputCount; i++)
  {
    inputPins[i] = i;
    inputValues[i] = 0;
  }
}

void loop() 
{
  for (int i = 0; i < inputCount; i++)
  {
    int inputValue = analogRead(inputPins[i]);
    if (!EqualWithinTolerance(inputValue, inputValues[i]))
    {
      inputValues[i] = inputValue;
      requiresUpdate = true;
    }
  }
  if (requiresUpdate)
  {
    Serial.println(GetUpdateMessage());
    requiresUpdate = false;
  }
}

boolean EqualWithinTolerance(int newValue, int recordedValue)
{
  if (newValue >= recordedValue - TOLERANCE && newValue <= recordedValue + TOLERANCE)
  {
    return true;
  }
  return false;
}

String GetUpdateMessage()
{
  String message = "{";
  message += "\"address\":"; 
  message += address; 
  message += ", ";
  message += "\"command\":";
  message += UPDATE;
  message += ", ";
  message += "\"function\":";
  message += moduleFunction;
  message += ", "; 
  message += "\"values\":[";
  for (int i = 0; i < inputCount; i++)
  {
    message += inputValues[i];
    if (i < inputCount - 1)
    {
      message += ", ";
    }
  }
  message += "]";
  message += "}";
  return message;
}


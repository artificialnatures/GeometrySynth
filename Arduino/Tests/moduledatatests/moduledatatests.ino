/* Send test data via USB
 * Data must be JSON formatted strings, corresponding to 
 * the layout of the ModuleData struct, which looks like this:
 * public struct ModuleData
 * {
 *     public int address;
 *     public ModuleFunction function;
 *     public Command command;
 *     public int[] values;
 *     public int connectedModuleAddress;
 * }
 * Requests should be formated corresponding to the
 * layout of the ModuleRequest struct, which looks like this:
 * public struct ModuleRequest
 * {
 *   public Command command;
 *   public int[] values;
 * }
 */
#import <ArduinoJson.h>

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
    UNLINK = 10,
    SELECT = 11,
    RISE = 12,
    FALL = 13,
    ON = 14,
    OFF = 15
};

//Module Definition:
int address = 10;
ModuleFunction moduleFunction = SINE_WAVE;
Command command = UPDATE;
int inputCount = 3;
int inputValues[10];

int send_interval = 20;
String message = "{\"message\":\"hello\"}";
char EOL = "\n";
StaticJsonBuffer<256> jsonBuffer;

void setup() 
{
  Serial.begin(19200);
  for (int i = 0; i < sizeof(inputValues); i++)
  {
    inputValues[i] = 0;
  }
}

void loop() 
{
  
  delay(send_interval);
}

void serialEvent()
{
  String request = Serial.readStringUntil(EOL);
  JsonObject& json = jsonBuffer.parseObject(request);
  command = json["command"].as<Command>();
  switch (command)
  {
    case CONNECT:
      break;
    case UPDATE:
      break;
  }
}

String GetUpdateMessage()
{
  String message = "{";
  message += "\"address\":"; 
  message += address; 
  message += ", ";
  message += "\"command\":";
  message += command;
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


#define USE_ARDUINO_INTERRUPTS true    // Set-up low-level interrupts for most acurate BPM math.
#include <PulseSensorPlayground.h>     // Includes the PulseSensorPlayground Library.   

//  Variables
const int PulseWire = 0;       // PulseSensor PURPLE WIRE connected to ANALOG PIN 0
const int LED13 = 13;          // The on-board Arduino LED, close to PIN 13.
int Threshold = 550;           // Determine which Signal to "count as a beat" and which to ignore.
                               // Use the "Gettting Started Project" to fine-tune Threshold Value beyond default setting.
                               // Otherwise leave the default "550" value. 
                               
// these arrays are used to repeat particular loops such as count average heart rate once after running the program
int arr[1]; 
int stopper[1];

                               
PulseSensorPlayground pulseSensor;  // Creates an instance of the PulseSensorPlayground object called "pulseSensor"


void setup() {   

  Serial.begin(9600);          // For Serial Monitor

  // Configure the PulseSensor object, by assigning our variables to it. 
  pulseSensor.analogInput(PulseWire);   
  pulseSensor.blinkOnPulse(LED13);       //auto-magically blink Arduino's LED with heartbeat.
  pulseSensor.setThreshold(Threshold);   

  // Double-check the "pulseSensor" object was created and "began" seeing a signal. 
   if (pulseSensor.begin()) {
    Serial.println("We created a pulseSensor Object !");  //This prints one time at Arduino power-up,  or on Arduino reset.  
  }
}



void loop() {
  //defining all required variables
 int cnt =0, num = 0;
 double myBPM =0; 
 double avgHR, myBPM2;

//This part of the code will be run only once to count average heart rate of a person
 long time0 = millis();//set current time for loop
 if (arr[0] == 0){
  Serial.print("Mesuring begins. Put your finger on the pulse sensor... \n"); //message to get ready for pulse measurment
  while (cnt < 10000){
    int HR = pulseSensor.getBeatsPerMinute();
    myBPM = myBPM + HR; // sum all beats
 // to eliminate 0 values which may sensor produce
    if (myBPM != myBPM2){//if the last heart rate was the same as previous do not increment their number
      num++;  
     }
    cnt = millis()-time0; //incrementation of cnt
    myBPM2 = myBPM; //assign previos value for if statement above
   }
   //average hear rate is calculated including first read value from the sensor
   avgHR = myBPM/(num+1);
   //outputting average heart rate
   Serial.print("Your average heart beat is ");
   Serial.print(avgHR);
   Serial.println(num);
   arr[0] = 1; //stops the loop makes conditions false forever to stop them in the next loop of the programm
 }

  //this part of the code indetify phobia by comparing current beat with average one
  double currentBPM = pulseSensor.getBeatsPerMinute();
  if (currentBPM > avgHR+5 && stopper[0] == 0){ //if one high value was indentified search for others
    long startTime = millis(); 
    int count = 0, numHighBeats;
    //counts how many there were values above the average
    while(count < 10000){
      currentBPM = pulseSensor.getBeatsPerMinute();
      if(currentBPM > avgHR+5){
        numHighBeats++;
       }
       count = millis() - startTime;
     }
    // if these values are more than 5 then the person has phobia
    if (numHighBeats >= 5){
      Serial.print("Your phobia has been identified\n");
      stopper[0] = 1;
     }
  }

}

  

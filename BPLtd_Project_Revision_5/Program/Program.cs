// ----------------------------------------------------------
// 'Broken Petrol Ltd' Petrol Station Management Application
//  Version: 0
// ----------------------------------------------------------
//  Code by Jacob Lummis
//  ARU-P SID: 2213276
// ----------------------------------------------------------
// Main Program File

// Initialise user interface and subordinate threads
userInterface thisInterface = new userInterface();
thisInterface.userChoice = 0;
Thread Navigator = new Thread(thisInterface.keyReader);
int vehicleCount = 0;

// Declare vehicle list and index vehicle
List<vehicle> vehiclePool = new List<vehicle>();
vehicle indexVehicle = new vehicle();

// Initialise Timer
System.Timers.Timer aTimer;

// Main Program Body
Console.WriteLine("");
Navigator.Start();
while(thisInterface.userChoice != 1)
{
    // Call vehicle generating timer
    setTimer();


    // 
    aTimer.Stop();
    aTimer.Dispose();
}

Console.WriteLine("\n\n\tPress any key to exit.");
Console.ReadKey();

// Program Methods
void setTimer()
{
    aTimer = new System.Timers.Timer(1500);
    //
    aTimer.Elapsed += OnTimedEvent;
    aTimer.AutoReset = true;
    aTimer.Enabled = true;
}

void OnTimedEvent()
{
    indexVehicle = new vehicle();
    vehiclePool.Add(indexVehicle);
}
// ----------------------------------------------------------
// 'Broken Petrol Ltd' Petrol Station Management Application
//  Version: 0
// ----------------------------------------------------------
//  Code by Jacob Lummis
//  ARU-P SID: 2213276
// ----------------------------------------------------------
// Main Program File:
// Call necessary 'using' directives
using System.Timers;

// Declare Program Variables
ConsoleKeyInfo userKey;
int userChoice = 0, vehicleCount = 0, left, top, totalFuelDispensed = 0, totalVehiclesServiced = 0;
bool validKeyPress = false, validKeyPress2 = false, scrnRefresh = true;
string[] pumpStatus = new string[9]{"OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN"};

//-----------------------------------------------------------------------------------------------------------
//// Previous Version Artifact, when I didn't realise I was passing full List access
//// to a method in the Pump class. I thought I was only passing a copy, and thus was 
//// attempting to empty the list twice. Left in as reminder
//// vehiclePool.Add(indexVehicle = new vehicle(1000));
//-----------------------------------------------------------------------------------------------------------

// The Fuel Station Forecourt is represented as a One-Dimensional Array of Pump Objects
// In Later versions, the Array Initialiser could be tied to an "Admin" adjustable config file option.
pump[] pumps = new pump[9];

// Populate the Forecourt array
for(int spot = 0; spot < pumps.Length; spot++)
{
    pumps[spot] = new pump(spot + 1);
}

// Generate Two 1-Dimensional integer arrays for tallying during shift
int[] currentVehiclesServed = new int[pumps.Length], currentFuelDispensed = new int[pumps.Length];

// Declare the Index Vehicle, and then the Vehicle Pool List as a List of Vehicle Objects.
vehicle indexVehicle = new vehicle();
List<vehicle> vehiclePool = new List<vehicle>();
// Declare Vehicle Queue as List of Vehicle Objects.
List<vehicle> vehicleQueue = new List<vehicle>();

// Initialise Vehicle Generation Timer
System.Timers.Timer vehiclePoolTimer;

// Declare new Threads to run 'in-parallel' with Main Program
Thread Navigator = new Thread(keyReader);
Thread Tally = new Thread(fuelTally);

// Main Program Body:
// Start Additional Threads
Navigator.Start();
Tally.Start();

// Program Start Screen Fluff
Console.WriteLine("\n\n\tPress Enter to start shift,\n\tEscape to exit.\n");   

// Start/Call vehicle generation timer
setTimer();
while(userChoice != 1)
{
    while((validKeyPress != true) && (userChoice != 2)){}
    // Early Program Exit Check
    if(userChoice == 1)
    {
        break;
    }
    Console.Clear();
    // Program Shift Loop
    while(validKeyPress2 != true)
    {
        (left, top) = Console.GetCursorPosition();
        Console.WriteLine("\n\tPress Exit to End Shift.");
        Console.WriteLine("\n\tQueue: {0}\t\tTotal Vehicle Pool: {1}", vehicleQueue.Count(), vehicleCount);
        Console.WriteLine("\tPump 1: {0}\tPump 2: {1}\tPump 3: {2}\n\tPump 4: {3}\tPump 5: {4}\tPump 6: {5}\n\tPump 7: {6}\tPump 8: {7}\tPump 9: {8}\n", pumpStatus[0], pumpStatus[1], pumpStatus[2], pumpStatus[3], pumpStatus[4], pumpStatus[5], pumpStatus[6], pumpStatus[7], pumpStatus[8]);
        Console.WriteLine("\tVehicles Serviced: {0} \n\tTotalFuelDispensed: {1}", Convert.ToString(totalVehiclesServiced),  Convert.ToString(totalFuelDispensed));
        Console.SetCursorPosition(left, top);
        scrnRefresh = true;
        while(scrnRefresh != false)
        {
            if(vehiclePool.Count() > 1)
            {
                while(vehicleQueue.Count() != 1)
                {
                    vehicleQueue.Add(vehiclePool[1]);
                    vehiclePool.Remove(vehiclePool[1]);
                }
            }
            // Check if Pumps are Busy, Couldn't figure out how to pass access to
            // the string array down to a timer method, something to look into.
            for(int i = 0; i < pumps.Length; i++)
            {
                if(pumps[i].getStatus() == false)
                {
                    pumpStatus[i] = "OPEN";
                }
            }
            // Wait for 40 milliseconds, Resulting in 25 Frames Per Second
            Thread.Sleep(40);
            scrnRefresh = false;
        }
    }
}
// In the event of user leaving early, kill vehicle generation timer
if(userChoice == 1)
{
    vehiclePoolTimer.Stop();
    vehiclePoolTimer.Dispose();
}
// Exit Screen Clear & Window Hold
Console.Clear();
Console.WriteLine("\n\n\tPress any key to exit.\n");
Console.ReadKey();

// Program Methods:
// Vehicle Generation Timer & Timer Elapsed Event Methods
void setTimer()
{
    vehiclePoolTimer = new System.Timers.Timer(1500);
    // Timer Settings
    vehiclePoolTimer.Elapsed += OnTimedEvent;
    vehiclePoolTimer.AutoReset = true;
    vehiclePoolTimer.Enabled = true;
}
void OnTimedEvent(Object source, ElapsedEventArgs e)
{
    indexVehicle = new vehicle(vehicleCount);
    vehiclePool.Add(indexVehicle);
    if(vehicleCount >= 80)
    {
        vehiclePoolTimer.Stop();
        vehiclePoolTimer.Dispose();
        // Console.WriteLine(vehicleCount);
    }
    else
    {
        // Console.WriteLine(vehicleCount);
        vehicleCount++;
    }
}
// Meanwhile In a seperate Thread, the Fuel Station's
// Statistics are tallied
void fuelTally()
{
    while(userChoice != 1)
    {
        for(int i = 0; i < pumps.Length; i++)
        {
            currentFuelDispensed[i] = pumps[i].getFuelDispensed();
            currentVehiclesServed[i] = + pumps[i].getServicedVehiclesNum();
        }
        totalFuelDispensed = currentFuelDispensed.Sum();
        totalVehiclesServiced = currentVehiclesServed.Sum();
    }
}
// Key Registry Method, Run on seperate Thread for responsiveness.
void keyReader()
{
    while(userChoice != 1)
    {
        // Registers user input without writing to screen.
        userKey = Console.ReadKey(true);
        switch(userKey.Key)
        {
            // Program Exit Condition Case
            // and Contexts.
            case ConsoleKey.Escape:
            {
                if(validKeyPress != true)
                {
                    validKeyPress = true;
                    userChoice = 1;
                }
                else if(validKeyPress2 != true)
                {
                    validKeyPress2 = true;
                    userChoice = 1;
                }
                else
                {

                }
                break;
            }
            // Condition to get past the start screen.
            case ConsoleKey.Enter:
            {
                if(validKeyPress != true)
                {
                    validKeyPress = true;
                    userChoice = 2;
                }
                break;
            }
            // Primary Program Logic Cases:
            // Register's either the main 1 to 9 keys
            // or their Numpad equivelants as valid input.
            // If corresponding pump (Converting from a count
            // starting at 1 to a count starting at zero)
            // is free AND there is a vehicle in the queue;
            // then set the Pump's display status to "BUSY"
            // and call that pumps vehicle assignment method,
            // passing in access to the Main Program's vehicleQueue List.
            case((ConsoleKey.D1) or (ConsoleKey.NumPad1)):
            {
                if(pumps[0].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[0] = "BUSY";
                            pumps[0].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D2) or (ConsoleKey.NumPad2)):
            {
                if(pumps[1].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[1] = "BUSY";
                            pumps[1].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D3) or (ConsoleKey.NumPad3)):
            {
                if(pumps[2].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[2] = "BUSY";
                            pumps[2].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D4) or (ConsoleKey.NumPad4)):
            {
                if(pumps[3].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[3] = "BUSY";
                            pumps[3].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D5) or (ConsoleKey.NumPad5)):
            {
                if(pumps[4].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[4] = "BUSY";
                            pumps[4].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D6) or (ConsoleKey.NumPad6)):
            {
                if(pumps[5].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[5] = "BUSY";
                            pumps[5].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D7) or (ConsoleKey.NumPad7)):
            {
                if(pumps[6].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[6] = "BUSY";
                            pumps[6].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D8) or (ConsoleKey.NumPad8)):
            {
                if(pumps[7].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[7] = "BUSY";
                            pumps[7].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            case((ConsoleKey.D9) or (ConsoleKey.NumPad9)):
            {
                if(pumps[8].getStatus() != true)
                {
                        if(vehicleQueue.Count() == 1)
                        {
                            pumpStatus[8] = "BUSY";
                            pumps[8].assignVehicle(vehicleQueue);
                        }
                }
                break;
            }
            // Any unrecognised Key-presses do nothing
            default:
            {
                break;
            }
        }
    }
}
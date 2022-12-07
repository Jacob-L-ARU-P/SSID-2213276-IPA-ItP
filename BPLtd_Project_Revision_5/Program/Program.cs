// ----------------------------------------------------------
// 'Broken Petrol Ltd' Petrol Station Management Application
//  Version: 0
// ----------------------------------------------------------
//  Code by Jacob Lummis
//  ARU-P SID: 2213276
// ----------------------------------------------------------
// Main Program File:
// call using directives, define internal variables
using System.Timers;
int userChoice = 0;
ConsoleKeyInfo userKey;
bool validKeyPress = false;
bool validKeyPress2 = false;
bool scrnRefresh = true;
int vehicleCount = 0, left, top;
int totalFuelDispensed = 0;
int totalVehiclesServiced = 0;
// Declare vehicle list and index vehicle
vehicle indexVehicle = new vehicle();
List<vehicle> vehiclePool = new List<vehicle>();
// vehiclePool.Add(indexVehicle = new vehicle(1000));

//
pump[] pumps = new pump[9];
string[] pumpStatus = new string[9]{"OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN", "OPEN"};
for(int spot = 0; spot < pumps.Length; spot++)
{
    pumps[spot] = new pump(spot + 1);
}

int[] currentVehiclesServed = new int[pumps.Length];
int[] currentFuelDispensed = new int[pumps.Length];

// Vehicle Queue
List<vehicle> vehicleQueue = new List<vehicle>();
// vehicle newVehicle = new vehicle(999);
// vehicleQueue.Add(newVehicle);
// Initialise Timer
System.Timers.Timer aTimer;

// Initialise user interface and subordinate threads
// userInterface thisInterface = new userInterface();
// thisInterface.userChoice = 0;
// Thread Navigator = new Thread(thisInterface.keyReader);
Thread Navigator = new Thread(keyReader);
Thread Tally = new Thread(fuelTally);

// Main Program Body
Console.WriteLine("\n");
Navigator.Start();
Tally.Start();
Console.WriteLine("\tPress Enter to start shift,\n\tEscape to exit.");   
// Call vehicle generating timer
setTimer();
while(userChoice != 1)
{
    while((validKeyPress != true) && (userChoice != 2)){}

    if(userChoice == 1)
    {
        break;
    }

    Console.Clear();

    while(validKeyPress2 != true)
    {
        (left, top) = Console.GetCursorPosition();
        Console.WriteLine("\n\tPress Exit to End Shift.");
        Console.WriteLine("\n\tQueue: {0}", (vehicleQueue.Count()));
        Console.WriteLine("\tPump 1: {0}\tPump 2: {1}\tPump 3: {2}\n\tPump 4: {3}\tPump 5: {4}\tPump 6: {5}\n\tPump 7: {6}\tPump 8: {7}\tPump 9: {8}\n", pumpStatus[0], pumpStatus[1], pumpStatus[2], pumpStatus[3], pumpStatus[4], pumpStatus[5], pumpStatus[6], pumpStatus[7], pumpStatus[8]);
        Console.WriteLine("\tVehicles Serviced: {0} \n\tTotalFuelDispensed: {1}", Convert.ToString(totalVehiclesServiced),  Convert.ToString(totalFuelDispensed));
        Console.SetCursorPosition(left, top);
        scrnRefresh = true;
        while(scrnRefresh != false)
        {
            //
                if(vehiclePool.Count() > 1)
                {
                    while(vehicleQueue.Count() != 1)
                    {
                        vehicleQueue.Add(vehiclePool[1]);
                        vehiclePool.Remove(vehiclePool[1]);
                    }
                }
            for(int i = 0; i < pumps.Length; i++)
            {
                if(pumps[i].getStatus() == false)
                {
                    pumpStatus[i] = "OPEN";
                }
            }
            Thread.Sleep(40);
            scrnRefresh = false;
        }
    }
}

if(userChoice == 1)
{
    aTimer.Stop();
    aTimer.Dispose();
}

Console.Clear();
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

void OnTimedEvent(Object source, ElapsedEventArgs e)
{
    indexVehicle = new vehicle(vehicleCount);
    vehiclePool.Add(indexVehicle);
    if(vehicleCount >= 80)
    {
        aTimer.Stop();
        aTimer.Dispose();
        // Console.WriteLine(vehicleCount);
    }
    else
    {
        // Console.WriteLine(vehicleCount);
        vehicleCount++;
    }
}

void fuelTally()
{
    while(userChoice != 1)
    {
        for(int i = 0; i < pumps.Length; i++)
        {
            currentFuelDispensed[i] = pumps[i].getFuelDispensed();
            currentVehiclesServed[i] = + pumps[i].getVehiclesServiced();
        }
        Thread.Sleep(100);
        totalFuelDispensed = currentFuelDispensed.Sum();
        totalVehiclesServiced = currentVehiclesServed.Sum();
    }
}

// Key Registry
void keyReader()
{
    while(userChoice != 1)
    {
        userKey = Console.ReadKey(true);
        switch(userKey.Key)
        {
            // 
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
            case ConsoleKey.Enter:
            {
                if(validKeyPress != true)
                {
                    validKeyPress = true;
                    userChoice = 2;
                }
                break;
            }
            // Number keys and Numpad keys recognised
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
            // Unrecognised Key-press does nothing
            default:
            {
                break;
            }
        }
    }
}
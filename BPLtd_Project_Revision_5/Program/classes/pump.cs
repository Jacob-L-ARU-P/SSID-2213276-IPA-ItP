// ----------------------------------------------------------
// 'Broken Petrol Ltd' Petrol Station Management Application
//  Version: 0
// ----------------------------------------------------------
//  Code by Jacob Lummis
//  ARU-P SID: 2213276
// ----------------------------------------------------------
// Tell VS Code that it needs to use timers in this
// class file
using System.Timers;
// Pump Class:
// Main Functional Class
class pump
{
    // Pump Attributes:
    // a boolean and three integers. The ID lets the
    // program know *which* pump is which in their
    // list. The Boolean is used to track the state
    // of the pump, no surprises there.
    // The final two integers are local tallies, to
    // be summed back in the main program for display.
    int pumpID;
    bool isOccupied;
    int litresofFuelDispensed;
    int servicedVehiclesNum;

    // A List is possibly overkill here, as a pump
    // can only service ONE vehicle at a time,
    // however for the simplicity of .add and .remove
    // the use case feels appropriate.
    List<vehicle> vehicleBeingServed = new List<vehicle>();

    // Last Attribute, the Timer. In this version, it is
    // used to track how long each car takes to fill up
    // with fuel. @ a pump-rate of 1.5 litres/second,
    // over the brief-declared 8 seconds, the car will
    // have pumped a total of 12 litres.
    System.Timers.Timer pumpfuelTimer;

    // Pump Constructors:
    // Blank for program initialisation
    public pump(){}

    // Single variable constructor, initialises the
    // "Forecourt" for actual use.
    public pump(int iD)
    {
        pumpID = iD;
        isOccupied = false;
    }
    
    // Pump Methods:
    // The Get Methods, return current value of local instance variables
    public bool getStatus()
    {
        return isOccupied;
    }
        public int getFuelDispensed()
    {
        return litresofFuelDispensed;
    }
    public int getServicedVehiclesNum()
    {
        return servicedVehiclesNum;
    }

    // Primary Program Interaction Method:
    // Get's passed access to the main program's "vehicleQueue" list
    // allowing for in-method clearing of the vehicleQueue as the
    // specific vehicle gets assigned to the local Pump.
    // Also starts the Pump timer.
    public void assignVehicle(List<vehicle> queue)
    {
        vehicleBeingServed.Add(queue[0]);
        queue.Remove(queue[0]);
        isOccupied = true;
        pumpFuelingStart();
    }

    // The Fuel Timer Methods:
    // Control how the timer is set up and if it auto-repeats
    // along with what happens when the timer is elapsed.
    // In this case, clearing out the local Pump and adding
    // to the local tallies.
    public void pumpFuelingStart()
    {
        pumpfuelTimer = new System.Timers.Timer(8000);
        // Timer Settings
        pumpfuelTimer.Elapsed += pumpFuelingEnd;
        pumpfuelTimer.AutoReset = false;
        pumpfuelTimer.Enabled = true;
    }
    public void pumpFuelingEnd(Object source, ElapsedEventArgs e)
    {
        vehicleBeingServed.Remove(vehicleBeingServed[0]);
        isOccupied = false;
        litresofFuelDispensed = litresofFuelDispensed + 12;
        servicedVehiclesNum ++;
    }
}
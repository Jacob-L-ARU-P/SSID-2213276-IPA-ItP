//
using System.Timers;
class pump
{
    //
    bool occupied;
    int pumpID;
    int fuelDispensed;
    int vehiclesServiced;
    List<vehicle> vehicleBeingServed = new List<vehicle>();
    System.Timers.Timer pumpfuelTimer;
    //
    public pump(){}
    public pump(int iD)
    {
        pumpID = iD;
        occupied = false;
    }

    public void assignVehicle(List<vehicle> queue)
    {
        vehicleBeingServed.Add(queue[0]);
        queue.Remove(queue[0]);
        occupied = true;
        pumpFuelingStart();
    }

    public bool getStatus()
    {
        return occupied;
    }

    public void pumpFuelingStart()
    {
        pumpfuelTimer = new System.Timers.Timer(8000);
        //
        pumpfuelTimer.Elapsed += pumpFuelingEnd;
        pumpfuelTimer.AutoReset = false;
        pumpfuelTimer.Enabled = true;
    }

    public void pumpFuelingEnd(Object source, ElapsedEventArgs e)
    {
        vehicleBeingServed.Remove(vehicleBeingServed[0]);
        occupied = false;
        fuelDispensed = fuelDispensed + 12;
        vehiclesServiced ++;
    }

    public int getFuelDispensed()
    {
        return fuelDispensed;
    }

    public int getVehiclesServiced()
    {
        return vehiclesServiced;
    }

}
namespace CoopProject;


// Execution time tasktype size / station speed

public class Station
{
    private string StationID;
    private double Speed;
    private int Capacity;
    private char FIFOFLAG;
    private char MULTIFLAG;

    public Station(string stationId,double speed, int capacity, char multiflag , char fifoflag )
    {
        Capacity = capacity;
        Speed = speed;
        StationID = stationId;
        FIFOFLAG = fifoflag == 'Y' ? 'Y' : 'N';
        MULTIFLAG = multiflag == 'Y' ? 'Y' : 'N';
    }
    

    public int GetCapacity()
    {
        return Capacity;
    }
    
    public string GetStationID()
    {
        return StationID;
    }

    public double GetSpeed()
    {
        return Speed;
    }
}
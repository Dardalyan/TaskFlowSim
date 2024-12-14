namespace CoopProject;


// Execution time tasktype size / station speed

public class Station
{
    private string StationID { get; }
    private double Speed { get; }
    private int Capacity { get; }
    private char FIFOFLAG { get; }
    private char MULTIFLAG { get; }
    public Dictionary<string,string> ExecutableTaskInfo { get; set; }

    public Station(string stationId,double speed, int capacity, char multiflag , char fifoflag )
    {
        Capacity = capacity;
        Speed = speed;
        StationID = stationId;
        FIFOFLAG = fifoflag == 'Y' ? 'Y' : 'N';
        MULTIFLAG = multiflag == 'Y' ? 'Y' : 'N';
        ExecutableTaskInfo = new Dictionary<string, string>();
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
    
    public char GetFifoflag(){return FIFOFLAG;}
    public char GetMultiflag(){return MULTIFLAG;}
}
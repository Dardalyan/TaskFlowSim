namespace CoopProject;


// Execution time tasktype size / station speed

public class Station
{
    private string StationID { get; }
    private int Capacity { get; }
    private char FIFOFLAG { get; }
    private char MULTIFLAG { get; }
    public Dictionary<string,KeyValuePair<string,string>> ExecutableTaskInfo { get; set; }

    public Station(string stationId, int capacity, char multiflag , char fifoflag )
    {
        Capacity = capacity;
        StationID = stationId;
        FIFOFLAG = fifoflag == 'Y' ? 'Y' : 'N';
        MULTIFLAG = multiflag == 'Y' ? 'Y' : 'N';
        ExecutableTaskInfo = new Dictionary<string, KeyValuePair<string, string>>();
    }
    

    public int GetCapacity()
    {
        return Capacity;
    }
    
    public string GetStationID()
    {
        return StationID;
    }
    
    public char GetFifoflag(){return FIFOFLAG;}
    public char GetMultiflag(){return MULTIFLAG;}
}
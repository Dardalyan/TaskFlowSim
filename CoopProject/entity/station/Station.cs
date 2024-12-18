using CoopProject.entities;

namespace CoopProject;


public class Station :Entity 
{
    private int Capacity { get; }
    private char FIFOFLAG { get; }
    private char MULTIFLAG { get; }
    public Dictionary<string,KeyValuePair<string,string>> ExecutableTaskInfo { get; set; }

    public Station(string stationID, int capacity, char multiflag , char fifoflag ):base(stationID)
    {
        Capacity = capacity;
        FIFOFLAG = fifoflag == 'Y' ? 'Y' : 'N';
        MULTIFLAG = multiflag == 'Y' ? 'Y' : 'N';
        ExecutableTaskInfo = new Dictionary<string, KeyValuePair<string, string>>();
    }

    public int GetCapacity()
    {
        return Capacity;
    }
    
    public char GetFifoflag(){return FIFOFLAG;}
    public char GetMultiflag(){return MULTIFLAG;}
}
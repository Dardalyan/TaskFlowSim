namespace CoopProject.facility;

public class SCreator : ICreator<Station>
{
    public Station CreateEntity(object information)
    {

        KeyValuePair<string, Dictionary<string, string>> station;
        try
        {
            station = (KeyValuePair<string, Dictionary<string, string>>)information;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
        
        
        Station s = new Station(station.Key,Convert.ToInt32(station.Value["max_capacity"]),
            Convert.ToChar(station.Value["MULTIFLAG"]),Convert.ToChar(station.Value["FIFOFLAG"]));
        foreach (var stationData in station.Value)
        {
            if (!stationData.Key.Equals("max_capacity") && !stationData.Key.Equals("MULTIFLAG") && !stationData.Key.Equals("FIFOFLAG"))
            {
                s.ExecutableTaskInfo.Add(stationData.Key, new KeyValuePair<string, string>("speed", stationData.Value));
            }
        }

        return s;
    }
}
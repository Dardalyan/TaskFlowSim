// See https://aka.ms/new-console-template for more information

using CoopProject;

public class Program
{
    public static List<Station> Stations = new List<Station>();
    public static void Main(String[] args)
    {
        ParseAndLogFlow();
    }
    
    private static string DirectoryOfWorkflow()
    {
        const char slash = '/';
        List<string>dirs = Environment.CurrentDirectory.Split('/').ToList();
        string directory = "/";
        dirs.RemoveAt(0);
        for (int i = 0; i < 3; i++)
        {
            dirs.RemoveAt(dirs.Count-1);
        }
        dirs.ForEach(i=> directory+=$"{i}"+slash);
        directory.Remove(directory.Length - 1);
        directory += "WorkFlow.txt";
        
        return directory;

    }

    private static Dictionary<int,string> ReadLinesOfWorkFlow()
    {
        string line;
        Dictionary<int, string> lineByLine = new Dictionary<int, string>();
        try
        {
            StreamReader sr;
            try
            {
                sr = new StreamReader(DirectoryOfWorkflow());
            }
            catch (Exception e)
            {
                throw new Exception("Workflow.txt file cannot open");
            }

            line = sr.ReadLine()!;

            int lineNumber = 1;
            while (line != null!)
            {
                if (line[0] == '(' && line.Contains("TASKTYPES"))
                    lineByLine.Add(lineNumber, line);

                if (line[0] == '(' && line.Contains("JOBTYPES"))
                    lineByLine.Add(lineNumber, line);

                if (line[0] == '(' && line.Contains("STATIONS"))
                    lineByLine.Add(lineNumber, line);

                line = sr.ReadLine()!;
                lineNumber++;
            }

        }
        catch (Exception e)
        {
            lineByLine = null!;
            Console.WriteLine(e.Message);
        }
        
        return lineByLine;
    }

    private static void ParseAndLogFlow()
    {
        Dictionary<int, string> lineByLine = ReadLinesOfWorkFlow();

        Dictionary<string, double> taskTypes = new Dictionary<string, double>();
        Dictionary<string,double> jobTypes = new Dictionary<string,double>();
        Dictionary<string,double> stations = new Dictionary<string,double>();

        //Line by line, this loop parses tasktypes, jobtypes and stations and put them into unique string lists.
        //If there is a problem in the file format, prints the warning.
        int columnCount = 0;
        foreach (var pairs in lineByLine)
        {
            columnCount++;
            if (pairs.Value.Contains("TASKTYPES"))
            {
                string line = pairs.Value;
                line = line[0] == '(' ? line.Remove(0, 1) : line;
                line = line[line.Length-1] == ')' ?  line.Remove(line.Length-1, 1) : line;
                
                // Parsing each data into a list
                List<string> parsedLine = line.Split(" ").ToList();
                // Temporary list to control number of occurance of the data in the original list. NOTE: in order to log just once !
                List<string> tempParsedLine = new List<string>();
                
                parsedLine.ForEach(i=> Console.Write(i+" "));

                // Enumerator to move through the list item by item
                IEnumerator<string> enumerator = parsedLine.GetEnumerator();
                // To keep the previous data 
                string prevData = parsedLine[0] == "TASKTYPES" ? parsedLine[0] : "";
                
                int rowCount = 0;
                
                while (enumerator.MoveNext())
                {
                    tempParsedLine.Add(enumerator.Current);
                    // Pass the first one (it is the title or the class name like TASKTYPES or STATIONS)
                    if (enumerator.Current.Equals("TASKTYPES"))
                    {
                        rowCount += "TASKTYPES".Length;
                        continue;
                    }
                    
                    // Check the item whether the format of tasktypeID is correct or not
                    if (enumerator.Current.Contains('T') && !enumerator.Current[0].Equals('T'))
                    {
                        LogWarning($"Invalid tasktypeID: {enumerator.Current}");
                        
                        // Correct the invalid tasktypeID 
                        var charList = enumerator.Current.ToCharArray();
                        
                        // Adjusting the invalid tasktype ID
                        string id = "T";
                        foreach (var c in charList)
                        {
                            if (c != 'T')
                                id += c;
                        }
                        taskTypes.Add(id,0.0);
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }

                    // After the tasktypeID, if the current doesn't have 'T', then check whether the size number is unsigned or not.
                    if (!prevData.Equals("TASKTYPES") && prevData.Contains('T') && !enumerator.Current.Contains('T'))
                    {

                        bool isNegative = enumerator.Current.Contains('-') && enumerator.Current[0] == '-';
                        string unsignedSize = "";
                        if (isNegative) 
                        { 
                            LogWarning($"Invalid Task Size for TaskID:{prevData} Size:{enumerator.Current}");
                            // Adjusting the invalid size of the current task
                            unsignedSize = enumerator.Current.Remove(0,1);
                        }
                        else
                        {
                            unsignedSize = enumerator.Current;
                        }
                        double size = Convert.ToDouble(unsignedSize);
                        
                        // Check if that tasktypeID is defined in the dictionary, if it is, then update the value
                        if (!taskTypes.Keys.Contains(prevData))
                            taskTypes.Add(prevData, size);
                        else taskTypes[prevData] = size;
                        
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }

                    // Check whether the current data is listed before or not
                    if (tempParsedLine.Count(i=>i == enumerator.Current) > 1)
                    {
                        LogWarning($"The Task Type:{enumerator.Current} is listed more than one");
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }

                    if(enumerator.Current.Contains('T'))
                        taskTypes.Add(enumerator.Current,0.0);
                    
                    prevData = enumerator.Current;
                    rowCount += enumerator.Current.Length + 1;
                   

                }
                Console.WriteLine("\nLog Of Adjusted TaskType Format");
               foreach (var keyValuePair in taskTypes)
               {
                   Console.WriteLine(keyValuePair.Key +":"+ keyValuePair.Value);
               }
                
            }
            else if (pairs.Value.Contains("JOBTYPES"))
            {
                
            }
            else if (pairs.Value.Contains("STATITONS"))
            {
                
            }
            else
            {
                //throw new Exception("\nWrong workflow text file format ! File content cannot be parsed !");
            }
            
        }
        
    }

    private static void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"\nWarning -> ");
        Console.ResetColor();
        Console.Write(message);
    }
    
    
}

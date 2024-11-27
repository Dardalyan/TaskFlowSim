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

        List<string> taskTypes = new List<string>();
        List<string> jobTypes = new List<string>();
        List<string> stations = new List<string>();

        //Line by line, this loop parses tasktypes, jobtypes and stations and put them into unique string lists.
        //If there is a problem in the file format, prints the warning.
        int columnCount = 0;
        foreach (var pairs in lineByLine)
        {
            columnCount++;
            if (pairs.Value.ToLower().Contains("tasktypes"))
            {
                string line = pairs.Value;
                line = line[0] == '(' ? line.Remove(0, 1) : line;
                line = line[line.Length-1] == ')' ?  line.Remove(line.Length-1, 1) : line;
                
                List<string> parsedLine = line.Split(" ").ToList();
                List<string> tempParsedLine = new List<string>();
                
                
                parsedLine.ForEach(i=> Console.Write(i+" "));

                IEnumerator<string> enumerator = parsedLine.GetEnumerator();
                string prevData = parsedLine[0] == "TASKTYPES" ? parsedLine[0] : "";
                
                int rowCount = 0;
                
                while (enumerator.MoveNext())
                {
                    tempParsedLine.Add(enumerator.Current);
                    if (enumerator.Current.Equals("TASKTYPES"))
                    {
                        rowCount += "TASKTYPES".Length;
                        continue;
                    }
                    

                    if (enumerator.Current.Contains('T') && !enumerator.Current[0].Equals('T'))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"\nWarning line:{columnCount} position:{rowCount} ->");
                        Console.ResetColor();
                        Console.Write($"Invalid Task ID {enumerator.Current}");
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }

                    if (!prevData.Equals("TASKTYPES") && prevData.Contains('T') && !enumerator.Current.Contains('T'))
                    {
                        try
                        {
                            bool isNegative = enumerator.Current.Contains('-') && enumerator.Current[0] == '-';
                            if (isNegative)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"\nWarning line:{columnCount} position:{rowCount} ->");
                                Console.ResetColor();
                                Console.Write($"Invalid Task Size for TaskID:{prevData} Size:{enumerator.Current}");
                            }

                            //double size = Convert.ToDouble(enumerator.Current);
                            rowCount += enumerator.Current.Length + 1;
                            prevData = enumerator.Current;
                            continue;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    if (tempParsedLine.Count(i=>i == enumerator.Current) > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"\nWarning -> ");
                        Console.ResetColor();
                        Console.Write($"The Task Type:{enumerator.Current} is listed more than one");
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }

                    prevData = enumerator.Current;
                    rowCount += enumerator.Current.Length + 1;
                   

                }
            }
            else if (pairs.Value.ToLower().Contains("jobtypes"))
            {
                
            }
            else if (pairs.Value.ToLower().Contains("statıons") || pairs.Value.ToLower().Contains("stations"))
            {
                
            }
            else
            {
                throw new Exception("Wrong workflow text file format ! \nFile content cannot be parsed !");
            }
            
        }
        
    }
    
    
}

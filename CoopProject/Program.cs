﻿// See https://aka.ms/new-console-template for more information

using System.Globalization;
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
        Dictionary<string,List<List<KeyValuePair<string,double>>>> jobTypes = new Dictionary<string,List<List<KeyValuePair<string,double>>>>();
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
                        LogWarning($"Invalid tasktypeID: {enumerator.Current}",rowCount,columnCount);
                        
                        // Correct the invalid tasktypeID 
                        var charList = enumerator.Current.ToCharArray();
                        
                        // Adjusting the invalid tasktype ID
                        string adjustedID = "T";
                        foreach (var c in charList)
                        {
                            if (c != 'T')
                                adjustedID += c;
                        }
                        taskTypes.Add(adjustedID,0.0);
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
                            LogWarning($"Invalid Task Size for TaskID:{prevData} Size:{enumerator.Current}",rowCount,columnCount);
                            // Adjusting the invalid size of the current task
                            unsignedSize = enumerator.Current.Remove(0,1);
                        }
                        else
                        {
                            unsignedSize = enumerator.Current;
                        }
                        // To convert decimal data by seperated '.' correctly into a double 
                        NumberFormatInfo provider = new NumberFormatInfo();
                        provider.CurrencyDecimalSeparator = ".";
                        double size = Convert.ToDouble(unsignedSize,provider);
                        
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
                        LogWarning($"The Task Type:{enumerator.Current} is listed more than one",rowCount,columnCount);
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
                // SHOWING THE RESULTS OF TASKTYPES
                foreach (var keyValuePair in taskTypes)
                {
                    Console.WriteLine(keyValuePair.Key +":"+ keyValuePair.Value);
                }
                
            }
            else if (pairs.Value.Contains("JOBTYPES"))
            {
                string line = pairs.Value;
                line = line[0] == '(' ? line.Remove(0, 1) : line;
                line = line[line.Length-1] == ')' ?  line.Remove(line.Length-1, 1) : line;
                
                
                // Parsing each data into a list
                List<string> parsedLine = line.Split("JOBTYPES ").ToList();
                parsedLine.RemoveAt(0); // removes the first index -> which is ["JOBTYPES"]
                Console.WriteLine(parsedLine[0]);

                // Remove the ...) (... occurance except the first '(' and the last ')' 
                parsedLine = parsedLine[0].Split(") (").ToList();
                // remove the '('
                parsedLine[0] = parsedLine[0].Remove(0, 1);
                parsedLine[parsedLine.Count-1]= parsedLine[parsedLine.Count-1].Remove(parsedLine[parsedLine.Count-1].Length-1, 1);
                
                // Merge the seperated data into a string line
                IEnumerator<string> enumerator = parsedLine.GetEnumerator();
                line = "";
                while (enumerator.MoveNext())
                {
                    line += enumerator.Current+" ";
                }
                
                // Now like we did in the tasktypes we can move on 
                parsedLine = line.Split(" ").ToList();
                enumerator = parsedLine.GetEnumerator();
                
                // To keep the previous data 
                string prevData = "";
                int rowCount = "JOBTYPES".Length+1;
                Dictionary<string,int> currentJobIDOccurance = new Dictionary<string, int>();
                string currentJobID = "";// This will reset in each occurance of jobTypeID 
                while (enumerator.MoveNext()  )
                {
                    
                    // Adjusting JobTypeID if it has an invalid text format
                    if (enumerator.Current.Contains('J') && !enumerator.Current[0].Equals('J'))
                    {
                        LogWarning($"Invalid jobTypeID: {enumerator.Current}",rowCount,columnCount);
                        
                        // Correct the invalid jobTypeID 
                        var charList = enumerator.Current.ToCharArray();
                        
                        // Adjusting the invalid jobTypeID
                        string adjustedID = "J";
                        foreach (var c in charList)
                        {
                            if (c != 'J')
                                adjustedID += c;
                        }
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        currentJobID = adjustedID;
                        // We check the occurence of each jobTypeID . EX: if J1 occures only once and J2 occurs 2 times then we hold {J1:1, J2:2} as a Dict.
                        try
                        {
                            currentJobIDOccurance.Add(adjustedID,0);
                        }
                        catch
                        {
                            currentJobIDOccurance[adjustedID]++;
                        }
                        //Here we are trying to put jobtypeID into dictionary, if the key is already exist, then it throws an error. 
                        try
                        {
                            jobTypes.Add(adjustedID, new List<List<KeyValuePair<string, double>>>());
                            jobTypes[adjustedID].Add(new List<KeyValuePair<string, double>>());
                        }
                        //We do not need to catch error, only thing we are doing here adding another option for that jobtypeID
                        catch
                        {
                            jobTypes[adjustedID].Add(new List<KeyValuePair<string, double>>());
                        }
                        continue;
                    }
                    // If there is no problem in jobTypeID format
                    if (enumerator.Current.Contains('J') && enumerator.Current[0].Equals('J'))
                    {
                        // We check the occurence of each jobTypeID . EX: if J1 occures only once and J2 occurs 2 times then we hold {J1:0, J2:1} as a Dict.
                        try
                        {
                            currentJobIDOccurance.Add(enumerator.Current,0);
                        }
                        catch
                        {
                            currentJobIDOccurance[enumerator.Current]++;
                        } 
                        //Here we are trying to put jobtypeID into dictionary, if the key is already exist, then it throws an error.                        try
                        try
                        {
                            jobTypes.Add(enumerator.Current, new List<List<KeyValuePair<string, double>>>());
                            jobTypes[enumerator.Current].Add(new List<KeyValuePair<string, double>>());
                        }
                        //We do not need to catch error, only thing we are doing here adding another option for that jobtypeID
                        catch
                        {
                            jobTypes[enumerator.Current].Add(new List<KeyValuePair<string, double>>());
                        }

                        currentJobID = enumerator.Current;
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }
                    
                    // If taskTypeID doesn't exist in TaskTypes, we need to log the warning.
                    if (!taskTypes.Keys.Contains(prevData) && prevData.Contains('T'))
                    {
                        LogWarning($"The taskTypeID:{prevData} is not declared in TaskTypes", rowCount,columnCount);
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }

                    // If the current data is a taskTypeID, we need to add that id into sequence
                    if (enumerator.Current.Contains("T") && enumerator.Current[0].Equals('T'))
                    {
                        //Console.WriteLine($"Occurance of {currentJobID}:"+  currentJobIDOccurance[currentJobID]);
                        
                        jobTypes[currentJobID][currentJobIDOccurance[currentJobID]]
                            .Add(KeyValuePair.Create(enumerator.Current,taskTypes[enumerator.Current]));
                        
                    }
                    // if the current doesn't have 'T' and 'J', then check whether the size number is unsigned or not.
                    if (!prevData.Contains("J") && prevData.Contains('T') && !enumerator.Current.Contains('T'))
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
                        // To convert decimal data by seperated '.' correctly into a double 
                        NumberFormatInfo provider = new NumberFormatInfo();
                        provider.CurrencyDecimalSeparator = ".";
                        double size = Convert.ToDouble(unsignedSize,provider);
                        
                        // We are tyring to find in jobTypes(currentJobTypeID : [n'th option] [found index of pair.Key matched with taskTypeID])
                        // Then re-define that key-pair with {taskTypeID : size}
                        jobTypes[currentJobID][currentJobIDOccurance[currentJobID]]
                            [jobTypes[currentJobID][currentJobIDOccurance[currentJobID]]
                                .FindIndex(pair => pair.Key == prevData)] = KeyValuePair.Create(prevData,size);
                        
                        rowCount += enumerator.Current.Length + 1;
                        prevData = enumerator.Current;
                        continue;
                    }
                    
                    prevData = enumerator.Current;
                    rowCount += enumerator.Current.Length + 1;
                }
                
                // Look each jobtypeID
                foreach (var pair in jobTypes)
                {   // We need to check each option of current jobTypeID.  example: J1 's 1st option [T1:1, T2:2, T3:3]
                    foreach (var optionList in pair.Value)
                    {   // Now we'll log and make it correct
                        optionList.RemoveAll(keyValuePair =>
                        {   // All pairs will be removed, if size is not declared in both taskTypes and jobTypes
                            // In addition, we'll remove that taskType from taskTypes.
                            var p =  keyValuePair.Value == 0;
                            if (p)
                            {   // If p is true, then we need to log 
                                LogWarning($"{keyValuePair.Key} has no default size, either a default size must be declared in TASKTYPE list or the size must be declared within the job.");
                                taskTypes.Remove(keyValuePair.Key);
                            }
                            return p;
                        });
                    }
                }


            }
            else if (pairs.Value.Contains("STATIONS"))
            {
                
            }
            else
            {
                //throw new Exception("\nWrong workflow text file format ! File content cannot be parsed !");
            }
        }
        
        Console.WriteLine();
        foreach (var pair in jobTypes)
        {
            foreach (var optionList in pair.Value)
            {
                // SHOWING THE RESULTS OF JOBTYPES
                Console.WriteLine(pair.Key+":"+ $"Option {pair.Value.IndexOf(optionList)+1}");
                Console.Write("[");
                foreach (var keyValuePair in optionList)
                    Console.Write($" {keyValuePair.Key}:{keyValuePair.Value} ");
                Console.WriteLine("]");
            }
        }
        
    }

    private static void LogWarning(string message,int rowNum = 0 , int colNum = 0)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        if(rowNum == 0 && colNum == 0)
            Console.Write("Warning -> ");
        else
            Console.Write($"\nWarning in line {colNum} and {rowNum}th position -> ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(message);
        Console.ResetColor();

    }
    
    
}

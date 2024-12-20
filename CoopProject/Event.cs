using System.Reflection.Metadata.Ecma335;

namespace CoopProject;
using System;
using System.Collections;


public class Event
{

    private Job Job;
    private Station Station;

    public Event(Job j, Station s)
    {
        Job = j;
        Station = s;
    }


    public static void Work(Job j, Station s,int time)
    {
        // Just increase visibility for each job 
        if(j.GetID() == "Job1") Console.ForegroundColor = ConsoleColor.Magenta;
        if(j.GetID() == "Job2") Console.ForegroundColor = ConsoleColor.Cyan;
        if(j.GetID() == "Job3") Console.ForegroundColor = ConsoleColor.DarkRed;
        if(j.GetID() == "Job4") Console.ForegroundColor = ConsoleColor.Yellow;
        if(j.GetID() == "Job5") Console.ForegroundColor = ConsoleColor.Green;
            
        // Get next one
        Task t = j.GetNextTask();
        if (t != null! )
        {
            Console.WriteLine($"{j.GetID()} processing...");
            s.AcceptTask(t);
            s.Execute(t,time);
        }
    }
    
    
}
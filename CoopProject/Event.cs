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

        Task t = j.GetNextTask();
        if (t != null!)
        {
            Console.WriteLine($"{j.GetID()} processing...");
            s.AcceptTask(t);
            s.Execute(t,time);
        }
    }
    
    
}
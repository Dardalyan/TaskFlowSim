namespace CoopProject;
using System;
using System.Collections;

public enum EventTypes 
{
    TaskStart,
    TaskEnd,
    JobStart,
    JobEnd
}

public class Event
{
    private List<int> EventTimes = new List<int>();
    private List<EventTypes> EventTypes = new List<EventTypes>();
    private List<String> EventIDs = new List<String>();
    
    public Event() {}

    //Finds and returns which station can execute the task of the given job
    public Station AssignJobs(List<Station> stations, Job job)
    {
        Task task = job.GetNextTask();
        List<Station> availableStations = new List<Station>();

        //Iterate through each station to find those capable of executing the task
        foreach (Station station in stations)
        {
            //If the station supports the task type, add it to the list of available stations
            if (station.AcceptedTaskTypes.Contains(task.GetTaskType()))
            {
                availableStations.Add(station);
            }
        }

        //If there are available stations capable of executing the task
        if (availableStations.Count != 0)
        {
            Random random = new Random();

            //Choose a random available station from the list
            return availableStations.ElementAt((int)random.NextInt64(availableStations.Count));
        }

        // If no station is capable of executing the task, return null
        return null;
    }
    
    public bool Update(int time) 
    {
        //Check if any event matches the given time
        foreach (int eventTime in EventTimes) 
        {
            if (eventTime == time) 
            {
                return true; //Event occurs at the given time
            }
        }
        return false; //No event occurs at the given time
    }

    public List<EventTypes> GetEventTypes(int time)
    {
        List<EventTypes> Output = new List<EventTypes>();

        //Iterate through all events to find those occurring at the given time
        for (int i = 0; i < EventTimes.Count(); i++)
        {
            int EventTime = EventTimes.ElementAt(i);
            if (EventTime == time)
            {
                Output.Add(EventTypes.ElementAt(i)); //Add the corresponding event type to the output list
            }
        }
        
        return Output; //Return the list of event types occurring at the given time
    }
    
    public List<String> GetEventID(int time)
    {
        List<String> Output = new List<String>();

        //Iterate through all events to find those occurring at the given time
        for (int i = 0; i < EventTimes.Count(); i++)
        {
            int EventTime = EventTimes.ElementAt(i);
            if (EventTime == time)
            {
                Output.Add(EventIDs.ElementAt(i)); // Add the corresponding event ID to the output list
            }
        }
        
        return Output; //Return the list of event IDs occurring at the given time
    }

    public void AddNewEvent(int Time, EventTypes EventType, String ID)
    {
        //Find the appropriate position to insert the new event based on its time
        for (int i = 0; i < EventTimes.Count(); i++)
        {
            int EventTime = EventTimes.ElementAt(i);
            if (EventTime > Time)
            {
                //Insert the new event at the correct position
                EventTimes.Insert(i, Time);
                EventTypes.Insert(i, EventType);
                EventIDs.Insert(i, ID);
                return;
            }
        }
        
        //If the new event is the last event at the queue
        EventTimes.Add(Time);
        EventTypes.Add(EventType);
        EventIDs.Add(ID);
    }
    
}

namespace CoopProject;

public class EventQueue
{
    private List<Station> Stations;
    private List<int> StartTimes;
    private List<Job> Jobs;

    private Time Time = new Time();
    private Event Event = new Event();

    public EventQueue(List<Station> stations,List<Job>jobs)
    {
        Stations = stations;
        Jobs = jobs;
        StartTimes = new List<int>();
        
        Jobs.ForEach(job =>
        {
            StartTimes!.Add(job.GetStartTime());
        });
    }

    public void Start()
    {
        while (true)
        {
            Time.Tick();
            int CurrentTime = Time.GetCurrentTime();
            
            CheckStartTimes();

            foreach (Job Job in Jobs)
            {
                if (Job.CheckIfActive() == false || Job.CheckIfBusy())
                {
                    continue;
                }

                Station Station = Event.AssignJobs(Stations, Job);
                if (Station != null)
                {
                    Station.AddToQueue(Job);
                    Job.SetBusy(true);
                }
            }

            //Update stations
            foreach (Station Station in Stations)
            {
                Station.Work(CurrentTime, Event);
            }
            
            //Check and handle events
            if (Event.Update(CurrentTime)) {
                CheckEventState(CurrentTime, Event.GetEventTypes(CurrentTime), Event.GetEventID(CurrentTime));
            }
            
            if (CheckIfAllJobsAreDeactivated() && CurrentTime > GetLastStartTime())
            {
                break;
            }
        }
        //Print station utilization
        UtilizationAndTardiness();
    }
    
    //Check the start times of jobs and activate them if it is their time
    private void CheckStartTimes()
    {
        for (int i = 0; i < StartTimes.Count(); i++)
        {
            if (StartTimes.ElementAt(i) == Time.GetCurrentTime())
            {
                Job Job = Jobs.ElementAt(i);
                Job.Activate();
                
                Event.AddNewEvent(Time.GetCurrentTime(), EventTypes.JobStart, Job.GetID());
            }
        }
    }

    private int GetLastStartTime()
    {
        int LastTime = StartTimes.First();
        foreach (int StartTime in StartTimes)
        {
            if (StartTime > LastTime)
            {
                LastTime = StartTime;
            }
        }

        return LastTime;
    }

    private bool CheckIfAllJobsAreDeactivated()
    {
        foreach (Job Job in Jobs)
        {
            if (Job.CheckIfActive())
            {
                return false;
            }
        }

        return true;
    }

    private void CheckEventState(int CurrentTime, List<EventTypes> EventTypes, List<String> EventIDs)
    {
        for (int i = 0; i < EventTypes.Count(); i++)
        {
            EventTypes EventType = EventTypes.ElementAt(i);
            String EventID = EventIDs.ElementAt(i);
            
            //Handle event types
            switch (EventType)
            {
                case CoopProject.EventTypes.JobStart:
                    Console.WriteLine(EventID + " started.");
                    break;
                
                case CoopProject.EventTypes.JobEnd:
                    Console.WriteLine(EventID + " finished.");
                    break;
                
                case CoopProject.EventTypes.TaskStart:
                    String[] TextStart = EventID.Split("/");
                    Console.WriteLine(TextStart[0] + " " + TextStart[1] + " started at station " + TextStart[2]);
                    break;
                
                case CoopProject.EventTypes.TaskEnd:
                    String[] TextFinish = EventID.Split("/");
                    Console.WriteLine(TextFinish[0] + " " + TextFinish[1] + " finished at station " + TextFinish[2]);
                    break;
            }
            
            Console.WriteLine("Time: " + CurrentTime + ", Event type: " + EventType);
        }
    }
    
    private void UtilizationAndTardiness()
    {
        int CurrentTime = Time.GetCurrentTime();

        //print utilization
        foreach (Station Station in Stations)
        {
            Console.WriteLine("Station "+ Station.GetID() + "Utilization: " + (Station.GetActiveTime() / CurrentTime * 100));
        }

        List<string> JobIDs = new List<string>();
        List<List<double>> JobTardiness = new List<List<double>>();

        foreach (Job Job in Jobs)
        {
            bool Found = false;
            for (int i = 0; i < JobIDs.Count(); i++)
            {
                String JobID = JobIDs.ElementAt(i);
                if (JobID.Equals(Job.GetJobType().GetJobTypeID()))
                {
                    JobTardiness.ElementAt(i).Add(Job.GetJobTardiness());
                    Found = true;
                    break;
                }
            }

            if (Found == false)
            {
                List<double> TardinessList = new List<double>();
                TardinessList.Add(Job.GetJobTardiness());
                
                JobIDs.Add(Job.GetJobType().GetJobTypeID());
                JobTardiness.Add(TardinessList);
            }
        }

        for (int i = 0; i < JobIDs.Count(); i++)
        {
            String JobID = JobIDs.ElementAt(i);
            Console.WriteLine("Job "+JobID+ "average tardiness = " +JobTardiness.ElementAt(i)+ " minutes"); //should take average
        }
    }
}
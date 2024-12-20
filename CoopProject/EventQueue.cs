
using System.Collections;

namespace CoopProject;

public class EventQueue
{
    private List<Station> Stations;
    private List<int> StartTimes;
    private List<Job> Jobs;
    private List<Job> JobsInProcess = new List<Job>();

    private Time Time = new Time();


    public EventQueue(List<Station> stations, List<Job> jobs)
    {
        Stations = stations;
        Jobs = jobs;
        StartTimes = new List<int>();

        Jobs.ForEach(job => { StartTimes!.Add(job.GetStartTime()); });
    }

    public void Start()
    {
        while (true)
        {
            Time.Tick();

            if (Time.GetCurrentTime() == 50) break;


            // Get Current job  with a start time equals to the current time 
            var candidateJob = Jobs.FindAll(j => j.GetStartTime() == Time.GetCurrentTime()).FirstOrDefault()!;
            if (!JobsInProcess.Contains(candidateJob) && candidateJob != null! && !candidateJob.IsJobFinished())
                JobsInProcess.Add(Jobs.Find(j => j.GetStartTime() >= Time.GetCurrentTime())!);



            // We select executable task
            Station station = null!;

            IEnumerator<Job> enumerator = JobsInProcess.GetEnumerator();

            List<Station> stationsInProcess = new List<Station>();

            while (enumerator.MoveNext())
            {
                Job j = enumerator.Current;
                IEnumerator<Station> sEnumerator = Stations.GetEnumerator();

                while (sEnumerator.MoveNext())
                {
                    var s = sEnumerator.Current;
                    if (s.ExecutableTasks.Contains(j.GetNextTask()))
                    {
                        stationsInProcess.Add(s);
                        break;
                    }
                }
            }
            
            

            for (int i = 0; i < JobsInProcess.Count; i++)
            { 
                
                Event.Work(JobsInProcess[i], stationsInProcess[i],Time.GetCurrentTime());
            }
            
            /*
            // Current event
            Console.Write(" j: "+JobsInProcess.Count);
            Console.WriteLine(" s: "+ stationsInProcess.Count);
            */
        }
    }
}

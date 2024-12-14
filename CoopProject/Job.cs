namespace CoopProject;

public class Job
{
    private JobType JobType { get; }
    private int Duration { get; }
    private int StartTime { get; }
    private string JobID { get; }

    public Job(string jobId,int startTime, int duration, JobType jobType)
    {
        JobID = jobId;
        JobType = jobType;
        Duration = duration;
        StartTime = startTime;
        
    }
    
    public int GetDuration(){return Duration;}
    public int GetStartTime(){return StartTime;}
    
    public JobType GetJobType()
    {
        return JobType;
    }

    public string GetJobID()
    {
        return JobID;
    }


}
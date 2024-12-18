using CoopProject.entities;

namespace CoopProject;

public class Job : Entity
{
    private JobType JobType { get; }
    private int Duration { get; }
    private int StartTime { get; }

    public Job(string jobId,int startTime, int duration, JobType jobType):base(jobId)
    {
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
    


}
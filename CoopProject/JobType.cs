namespace CoopProject;

public class JobType
{
    private string JobTypeID;
    private double Duration;
    public Queue<Task> Tasks;

    public JobType(string jobTypeId, int duration = 2)
    {
        JobTypeID = jobTypeId;
        Duration = duration;
        Tasks = new Queue<Task>();
    }
    
    public string GetJobTypeID()
    {
        return JobTypeID;
    }

    public double GetDuration()
    {
        return Duration;
    }
}
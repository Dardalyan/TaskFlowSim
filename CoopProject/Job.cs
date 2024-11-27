namespace CoopProject;

public class Job
{
    private JobType JobType;
    private string JobID;

    public Job(string jobId, JobType jobType)
    {
        JobID = jobId;
        JobType = jobType;
    }
    
    public JobType GetJobType()
    {
        return JobType;
    }

    public string GetJobID()
    {
        return JobID;
    }


}
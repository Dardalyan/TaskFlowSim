using CoopProject.entities;

namespace CoopProject;

public class Job : Entity
{
    private JobType JobType { get; }
    private int Duration { get; }
    private int StartTime { get; }

    private bool IsFinieshed = false;

    private int Deadline { get; set; }

    public Job(string jobId, int startTime, int duration, JobType jobType) : base(jobId)
    {
        JobType = jobType;
        Duration = duration;
        StartTime = startTime;
        Deadline = StartTime + Duration;
    }

    public int GetDuration()
    {
        return Duration;
    }

    public int GetStartTime()
    {
        return StartTime;
    }

    public JobType GetJobType()
    {
        return JobType;
    }

    public int GetDeadline()
    {
        return Deadline;
    }

    public bool IsJobFinished()
    {
        return IsFinieshed;
    }

    // If the Task is Done, then remove it from the sequence and return the first value again until the list becomes empty.
    public Task GetNextTask()
    {
        if (JobType.TaskSequence[0].Count >= 1)
            if(JobType.TaskSequence[0][0].CheckFinished()) JobType.TaskSequence[0].RemoveAt(0);

        if (JobType.TaskSequence[0].Count == 0 || JobType.TaskSequence[0] == null!)
        {
            IsFinieshed = true;
            return null!;
        }
        return JobType.TaskSequence[0][0];


    }


}
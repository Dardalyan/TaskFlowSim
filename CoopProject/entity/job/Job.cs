using CoopProject.entities;

namespace CoopProject;

public class Job : Entity
{
    private JobType JobType { get; }
    private int Duration { get; }
    private int StartTime { get; }
    
    //!!!!
    private bool isActive;
    //!!!!
    private bool isBusy; //is the job currently getting done
    
    //!!!!
    public int DeadLine{private set; get; }

    public Job(string jobId,int startTime, int duration, JobType jobType):base(jobId)
    {
        JobType = jobType;
        Duration = duration;
        StartTime = startTime;
        DeadLine = CalculateDeadline();
    }
    
    public int GetDuration(){return Duration;}
    public int GetStartTime(){return StartTime;}
    
    public JobType GetJobType()
    {
        return JobType;
    }

    public int GetDeadline()
    {
        return DeadLine;
    }

    //!!!!!!!!
    private int CalculateDeadline()
    {
        return StartTime + Duration;
    }

    //!!!!!!!!
    //Get the next task to be executed
    public Task GetNextTask() 
    {
        foreach (var option in JobType.TaskSequence)
        {
            var sequenceInCurrentOption = option.Value;
            
            IEnumerator<Task> enumerator = sequenceInCurrentOption.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if(!enumerator.Current.CheckFinished())
                    return enumerator.Current;
            }
        }
        //job is finished
        return null;
    }

    //!!!!
    public bool CheckIfActive()
    {
        return isActive;
    }
    
    //!!!!!!
    public void Activate()
    {
        isActive = true;
    }
    
    //!!!!!
    public void Deactivate()
    {
        isActive = false;
    }

    //!!!!!!
    public bool CheckIfBusy()
    {
        return isBusy;
    }

    //!!!!!!
    public void SetBusy(bool Busy)
    {
        isBusy = Busy;
    }
    
    //!!!!
    public void SetFinishTime(int FinishTime)
    {
        this.DeadLine = FinishTime;
    }
    
    //!!!
    public int GetJobTardiness()
    {
        return DeadLine - CalculateDeadline();
    }


}
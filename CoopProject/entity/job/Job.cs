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
    private int FinishTime;

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

    //!!!!!!!!
    public int CalculateDeadline()
    {
        return StartTime + Duration;
    }

    //!!!!!!!!
    //Get the next task to be executed
    public Task GetNextTask() 
    {
        for(int x=0; x< JobType.TaskSequence.Count(); x++)
        {
            if (JobType.TaskSequence[x].ElementAt(0).CheckFinished() == false)
            {
                return JobType.TaskSequence[x].ElementAt(0);
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
        this.FinishTime = FinishTime;
    }
    
    //!!!
    public int GetJobTardiness()
    {
        return FinishTime - CalculateDeadline();
    }


}
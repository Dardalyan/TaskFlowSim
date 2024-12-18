using CoopProject.entities;

namespace CoopProject;


public class Station :Entity 
{
    private int Capacity { get; }
    private char FIFOFLAG { get; }
    private char MULTIFLAG { get; }
    public Dictionary<string,KeyValuePair<string,string>> ExecutableTaskInfo { get; set; }
    
    //!!!!
    public List<TaskType> AcceptedTaskTypes;
    //!!!
    public List<Task> ExecutableTasks;
    
    //!!!!!
    private List<Job> Queue = new List<Job>();
    
    //!!!!
    private List<Job> JobsBeingProcessed = new List<Job>();
    
    //!!!!
    private List<double> TaskTypeSpeeds;
    
    //!!!!
    private List<double> TaskTypesPlusMinus;
    
    //!!!!
    private List<int> TasksFinishTime;
    
    //!!!!
    private int TimeActive = 0;

    public Station(string stationID, int capacity, char multiflag , char fifoflag ):base(stationID)
    {
        Capacity = capacity;
        FIFOFLAG = fifoflag == 'Y' ? 'Y' : 'N';
        MULTIFLAG = multiflag == 'Y' ? 'Y' : 'N';
        ExecutableTaskInfo = new Dictionary<string, KeyValuePair<string, string>>();
        //TODO AcceptedTaskTypes, TaskTypeSpeeds, TaskTypesPlusMinus ExecutableTaskInfodan çekilecek.
    }

    public int GetCapacity()
    {
        return Capacity;
    }
    
    public char GetFifoflag(){return FIFOFLAG;}
    public char GetMultiflag(){return MULTIFLAG;}
    
    //!!!!
    //Add the job to the stations queue
    public void AddToQueue(Job Job)
    {
        Queue.Add(Job);
    }
    
    //!!!!!
    public void Work(int Time, Event Event)
    {
        List<Job> AvailableJobs = GetAvailableTasks();

        //Assign available jobs to JobsBeşngProcessed
        while (JobsBeingProcessed.Count() < Capacity)
        {
            //No available jobs
            if (!AvailableJobs.Any())
            {
                break;
            }
            
            //Decide on jobs based on fifo or earliest deadline
            Job Job = AvailableJobs.First();
            int Index = 0;
            
            //fifo = no, so find job with the earliest ddeadline
            if (FIFOFLAG == 'N')
            {
                int JobDeadLine = Job.CalculateDeadline(); //????

                for (int i = 0; i < AvailableJobs.Count(); i++)
                {
                    Job AvailableJob = AvailableJobs.ElementAt(i);

                    int AvailableJobDeadline = AvailableJob.CalculateDeadline();
                    if (AvailableJobDeadline < JobDeadLine) {
                        JobDeadLine = AvailableJobDeadline;
                        Job = AvailableJob;
                        Index = i;
                    }
                }
            }
            // Execute task and update job status
            JobsBeingProcessed.Add(Job);
            int TaskFinish = (int)ExecuteTask(Job.GetNextTask(), Time);
            TasksFinishTime.Add(TaskFinish);

            // Remove job from available list and queue
            AvailableJobs.RemoveAt(Index);
            Queue.Remove(Job);

            // Add task start and end events to event list
            String TaskID = Job.GetNextTask().GetID();
                Event.AddNewEvent(Time, EventTypes.TaskStart, Job.GetID() + " " + Job.GetJobType().GetJobTypeID() + "/" + TaskID + "/" + GetID());
                Event.AddNewEvent(TaskFinish, EventTypes.TaskEnd, Job.GetID() + " " + Job.GetJobType().GetJobTypeID() + "/" + TaskID + "/" + GetID());
        }
        
        if (JobsBeingProcessed.Any()) {
            TimeActive++;
        }

        // Update job status when tasks finish
        List<Job> JobsToRemove = new List<Job>();
        
        for (int i = 0; i < JobsBeingProcessed.Count; i++) {
            int TaskFinish = TasksFinishTime.ElementAt(i);

            if (TaskFinish == Time) {
                Job WorkingJob = JobsBeingProcessed.ElementAt(i);
                WorkingJob.GetNextTask().FinishTask();

                WorkingJob.SetBusy(false);

                JobsToRemove.Add(WorkingJob);
            }

        }
        
        // Remove finished jobs from workingJobs and queue
        foreach (Job Job in JobsToRemove) {
            TasksFinishTime.RemoveAt(JobsBeingProcessed.IndexOf(Job));
            JobsBeingProcessed.Remove(Job);
            Queue.Remove(Job);

            // Add job end event if all tasks are finished
            if (Job.GetNextTask() == null) {
                Event.AddNewEvent(Time, EventTypes.JobEnd, Job.GetID());
                Job.SetFinishTime(Time);
            }
        }
    }

    //!!!!
    //Returns the available tasks in the stations queue
    private List<Job> GetAvailableTasks()
    {
        List<Job> AvailableJobs = new List<Job>();

        //If the station is multiflag or no jobs are currently being processed, all tasks are available
        if (MULTIFLAG == 'Y' || !JobsBeingProcessed.Any())
        {
            foreach (Job Job in Queue)
            {
                AvailableJobs.Add(Job);
            }
        }
        else
        {
            
            //If a task is being executed, only the same type tasks as the current task are available
            String CurrentTaskType = JobsBeingProcessed.First().GetNextTask().GetTaskType().GetTaskTypeID();
            foreach (Job Job in Queue)
            {
                Task Task = Job.GetNextTask();
                if (Task.GetID().Equals(CurrentTaskType))
                {
                    AvailableJobs.Add(Job);
                }
            }
        }

        return AvailableJobs;
    }
    //!!!!
    //Execute a task and calculate the finish time
    private double ExecuteTask(Task Task, int Time)
    {
        int Index = ExecutableTasks.IndexOf(Task);

        double BaseSpeed = TaskTypeSpeeds.ElementAt(Index);
        double PlusMinus = TaskTypesPlusMinus.ElementAt(Index);

        double MinSpeed = BaseSpeed - (BaseSpeed * PlusMinus / 100);
        double MaxSpeed = BaseSpeed + (BaseSpeed * PlusMinus / 100);

        Random random = new Random();
        double Speed = MinSpeed + random.Next(0, 2) * (MaxSpeed - MinSpeed);
        double Duration = Task.GetSize() / Speed;

        return Duration + Time;
    }

    public int GetActiveTime()
    {
        return TimeActive;
    }
}
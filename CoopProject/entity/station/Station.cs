using CoopProject.entities;
namespace CoopProject;


public class Station :Entity 
{
    private int Capacity { get; }
    private char FIFOFLAG { get; }
    private char MULTIFLAG { get; }

    private  Queue<Task>AcceptedTasks { set; get; }
    private  Queue<Task> TasksOnWaiting { set; get; }
    
    private  Dictionary<Task,int> TaskStartTime  = new Dictionary<Task,int>();

    private Dictionary<Task, KeyValuePair<string, double>> _executableTaskInfo; // {instance:{"speed":"3"}}
    public Dictionary<Task,KeyValuePair<string,double>> ExecutableTaskInfo
    {
        get { return _executableTaskInfo;} set{ _executableTaskInfo=value;} 
    }
    
    //!!!
    public List<Task> ExecutableTasks { set; get; }
    public List<double> ExecutionSpeeds { set; get; } 
    

    public Station(string stationID, int capacity, char multiflag , char fifoflag, Action<Station>assignExecTinfo):base(stationID)
    {
        Capacity = capacity;
        FIFOFLAG = fifoflag == 'Y' ? 'Y' : 'N';
        MULTIFLAG = multiflag == 'Y' ? 'Y' : 'N';
        _executableTaskInfo = new Dictionary<Task, KeyValuePair<string, double>>();
        assignExecTinfo(this);

        AcceptedTasks = new Queue<Task>();
        TasksOnWaiting = new Queue<Task>();
        
        
        ExecutableTasks = new List<Task>();
        ExecutionSpeeds = new List<double>();
        
        _executableTaskInfo.Keys.ToList().ForEach(task =>
        {
            ExecutableTasks.Add(task);
        });

        foreach (var info in _executableTaskInfo)
        {
            double speed = info.Value.Value;
            //Console.WriteLine($"{info.Key.GetTaskType().GetTaskTypeID()} with {speed} in {ID} .");
            ExecutionSpeeds.Add(speed);
        }
        
        
        
    }
    
    
    private bool IsStationFull()
    {
        return Capacity == AcceptedTasks.Count;
    }

    public int GetCapacity()
    {
        return Capacity;
    }
    
    public char GetFifoflag(){return FIFOFLAG;}
    public char GetMultiflag(){return MULTIFLAG;}


    public void AcceptTask(Task task)
    {
        if (!IsStationFull() && ExecutableTasks.Contains(task))
        {
            AcceptedTasks.Enqueue(task);
            Console.WriteLine($"{task.GetTaskType().GetTaskTypeID()} accepted by {ID}.");
        }
        else if(!IsStationFull() && !ExecutableTasks.Contains(task) && MULTIFLAG == 'Y')
        {
            AcceptedTasks.Enqueue(task);
            Console.WriteLine($"{task.GetTaskType().GetTaskTypeID()} accepted by {ID}.");
            
        }
        else if (IsStationFull() && ExecutableTasks.Contains(task))
        {
            TasksOnWaiting.Enqueue(task);
            Console.WriteLine($"{task.GetTaskType().GetTaskTypeID()} is waiting in {ID}.");
        }
        else if (IsStationFull() && !ExecutableTasks.Contains(task)  && MULTIFLAG == 'Y')
        {
            TasksOnWaiting.Enqueue(task);
            Console.WriteLine($"{task.GetTaskType().GetTaskTypeID()} is waiting in {ID}.");
        }
    }
   
    //Execute a task and calculate the finish time
    public void Execute(Task task, int time)
    {
        int index = ExecutableTasks.IndexOf(task);
        
        if(!TaskStartTime.ContainsKey(task))
            TaskStartTime.Add(task,time);
        
        Console.WriteLine($"{task.GetTaskType().GetTaskTypeID()} entered in {TaskStartTime[task]}th second.");
        Console.WriteLine($"in {ID} , {task.GetTaskType().GetTaskTypeID() } is in process...");
        double executionDuration = (task.GetSize() / ExecutionSpeeds[index]);
        
        Console.WriteLine($"Duratiton of {task.GetTaskType().GetTaskTypeID()} in {ID} is {executionDuration} seconds.");
        
        Console.WriteLine($"{ID}'s Execution speed for {task.GetTaskType().GetTaskTypeID()}: "+ ExecutionSpeeds[index] + " second.");
        
        // If the time has come for the task, then remove it from the queue.
        if (Time.GetCurrentTime() - TaskStartTime[task] <= Convert.ToInt32(executionDuration))
            /*
             NOTE: We couldn't solve this equation problem 
             When we change the equation to == it starts infinite loop !! 
             */
        {
            AcceptedTasks.Dequeue().FinishTask();
            Console.WriteLine($"The execution of  {task.GetTaskType().GetTaskTypeID()} is finished.");

            if (TasksOnWaiting.Count != 0)
            {
                Task t = TasksOnWaiting.Dequeue();
                AcceptedTasks.Enqueue(t);
                Console.WriteLine($"{t.GetTaskType().GetTaskTypeID()} is now being processed.");
            }
        }
        Console.WriteLine("------------------------------------------------------------------------------");

        
    }
    
}
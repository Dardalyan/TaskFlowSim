namespace CoopProject;

public class Task
{
    private TaskType TaskType { get; }
    private double Size { get; }
    
    //!!!!!!
    private bool isFinished = false;
    //!!!!!!
    private String ID;
    //!!!!
    private static int IDCounter = 0;

    public Task(double size,TaskType taskType)
    {
        TaskType = taskType;
        Size = size;
        ID = IDCounter.ToString();
        IDCounter++;
    }

    public TaskType GetTaskType()
    {
        return TaskType;
    }

    public double GetSize()
    {
        return Size;
    }
    
    //!!!!
    public bool CheckFinished()
    {
        return isFinished;
    }
    
    //!!!!
    public void FinishTask()
    {
        isFinished = true;
    }
    
    //!!!!
    public String GetID()
    {
        return ID;
    }
}
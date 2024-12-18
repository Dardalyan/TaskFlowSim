namespace CoopProject;

public class Task
{
    private TaskType TaskType { get; }
    private double Size { get; }

    public Task(double size,TaskType taskType)
    {
        TaskType = taskType;
        Size = size;
    }

    public TaskType GetTaskType()
    {
        return TaskType;
    }

    public double GetSize()
    {
        return Size;
    }
}
namespace CoopProject;

public class Task
{
    private TaskType TaskType;
    private double Size;

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
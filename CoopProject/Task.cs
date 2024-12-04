namespace CoopProject;

public class Task
{
    private TaskType TaskType;
    private string TypeID;

    public Task(string taskId,TaskType taskType)
    {
        TaskType = taskType;
        TypeID = taskId;
    }

    public TaskType GetTaskType()
    {
        return TaskType;
    }

    public string GetTaskTypeID()
    {
        return TypeID;
    }
}
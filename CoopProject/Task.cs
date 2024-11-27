namespace CoopProject;

public class Task
{
    private TaskType TaskType;
    private string TaskID;

    public Task(string taskId,TaskType taskType)
    {
        TaskType = taskType;
        TaskID = taskId;
    }

    public TaskType GetTaskType()
    {
        return TaskType;
    }

    public string GetTaskID()
    {
        return TaskID;
    }
}
namespace CoopProject;

public class TaskType
{
    private string TaskTypeID;
    private int Size;

    public TaskType(string taskTypeId, int size)
    {
        TaskTypeID = taskTypeId;
        Size = size;
    }

    public string GetTaskTypeID()
    {
        return TaskTypeID;
    }
    
    public int GetSize()
    {
        return Size;
    }
}
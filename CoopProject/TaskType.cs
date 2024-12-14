namespace CoopProject;

public class TaskType
{
    private string ID { get; }

    public TaskType(string id)
    {
        ID = id;
    }
    
    public string GetTaskTypeID()
    {
        return ID;
    }
}
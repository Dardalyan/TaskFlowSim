namespace CoopProject;

public class Time
{
    private static int CurrentTime = 0;
    
    public static int GetCurrentTime()
    {
        return CurrentTime;
    }

    public void Tick()
    {
        CurrentTime++;
        Console.WriteLine($"Current time: {CurrentTime}");

    }
}
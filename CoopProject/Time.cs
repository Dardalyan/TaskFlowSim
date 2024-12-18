namespace CoopProject;

public class Time
{
    private int CurrentTime;

    public Time()
    {
        CurrentTime = 0;
    }

    public int GetCurrentTime()
    {
        return CurrentTime;
    }

    public void Tick()
    {
        CurrentTime = CurrentTime + 1;
    }
}
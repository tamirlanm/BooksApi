
public class RequestCounterService
{
    public int Count {get;private set;}
    public void Increment() => Count++;
    public Guid Id {get; } = Guid.NewGuid();
}
namespace MyWebApiApp.Interfaces
{
    public interface ITaskProgressService
    {
        Task HandleEventAsync(string userId, string eventType, int value);
    }
}

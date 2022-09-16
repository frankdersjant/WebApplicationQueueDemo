namespace WebApplicationQueue.Backgroundservice
{
    public interface IQueueStorage
    {
        Task CreateMessage(string message);
        Task<string> PeekMessage();
        Task DeleteMessage();
    }
}

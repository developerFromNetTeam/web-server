namespace web_server.IServices
{
    public interface ICustomLogger<T>
    {
        void LogInformation(string message);
        void LogError(string message);
    }
}

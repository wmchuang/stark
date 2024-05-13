namespace Stark.Starter.Web.Logger;

public interface IDatabaseLoggerStory
{
    public void SaveAsync(LogInfo logInfo);
}
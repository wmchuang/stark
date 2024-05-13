namespace Stark.Starter.Job.Attribute;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TriggerCronAttribute : System.Attribute
{
    public TriggerCronAttribute(string cron)
    {
        Cron = cron;
    }

    public string Cron { get; }
}
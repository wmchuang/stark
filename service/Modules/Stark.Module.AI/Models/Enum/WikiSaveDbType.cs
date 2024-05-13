using System.ComponentModel;

namespace Stark.Module.AI.Models.Enum;

public enum WikiSaveDbType
{
    /// <summary>
    /// Postgres
    /// </summary>
    [Description("Postgres")] Postgres = 0,

    /// <summary>
    /// Disk
    /// </summary>
    [Description("Disk")] Disk,

    /// <summary>
    /// Memory
    /// </summary>
    [Description("Memory")] Memory
}
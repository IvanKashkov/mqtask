using mqtask.Domain.Entities;

namespace mqtask.Persistence.Interfaces
{
    /// <summary>
    /// Just for extensibility
    /// </summary>
    public interface IDbSnapshotBuilder
    {
        DbSnapshot Build();
    }
}

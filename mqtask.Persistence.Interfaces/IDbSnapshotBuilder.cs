using mqtask.Domain.Entities;

namespace mqtask.Persistence.Interfaces
{
    public interface IDbSnapshotBuilder
    {
        DbSnapshot Build();
    }
}

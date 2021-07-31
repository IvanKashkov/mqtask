using mqtask.Domain;
using mqtask.Domain.Entities;

namespace mqtask.Application
{
    public static class DbSnapshotHolder
    {
        private static DbSnapshot _instance;

        public static DbSnapshot Instance => _instance;

        public static void Init(DbSnapshot snapshot)
        {
            _instance = snapshot;
        }
    }
}

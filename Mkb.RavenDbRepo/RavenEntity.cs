using System;

namespace Mkb.RavenDbRepo
{
    public abstract class RavenEntity
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; } = null;
    }
}
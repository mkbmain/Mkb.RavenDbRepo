using System;

namespace Mkb.RavenDbRepo.Entities
{
    public abstract class RavenEntity
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
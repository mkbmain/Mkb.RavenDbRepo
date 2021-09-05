using System;

namespace Mkb.RavenDbRepo.Tests.RaveReponAsyncTests
{
    public class Entity : RavenEntity
    {
        protected bool Equals(Entity other)
        {
            return Name == other.Name && Email == other.Email && Dob.Equals(other.Dob);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Email, Dob);
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
    }
}
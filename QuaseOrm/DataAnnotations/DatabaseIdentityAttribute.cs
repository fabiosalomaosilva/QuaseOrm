using System;

namespace QuaseOrm.DataAnnotations
{
    public class DatabaseIdentityAttribute : Attribute
    {
        public DatabaseIdentityAttribute()
        {
            this.IsActive = true;
        }

        public DatabaseIdentityAttribute(bool isActive)
        {
            this.IsActive = isActive;
        }

        public bool IsActive { get; set; }
    }
}
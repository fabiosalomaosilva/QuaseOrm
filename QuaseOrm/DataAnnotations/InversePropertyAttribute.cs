using System;

namespace QuaseOrm.DataAnnotations
{
    public class InversePropertyAttribute : Attribute
    {
        public InversePropertyAttribute(string property)
        {
            this.Property = property;
        }

        public string Property { get; set; }
    }
}
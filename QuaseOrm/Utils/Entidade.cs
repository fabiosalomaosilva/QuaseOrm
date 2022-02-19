using System.Collections.Generic;

namespace QuaseOrm.Utils
{
    public class Entity
    {
        public string Object { get; set; }
        public IList<string> ListEntities { get; set; }
    }
}
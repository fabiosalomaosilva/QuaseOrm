using System;
using System.Collections.Generic;
using System.Linq;

namespace QuaseOrm.Utils
{
    public class Entity
    {
        public string Object { get; set; }
        public IList<string> ListEntities { get; set; }
    }
}


using Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public DateTime EstablishDate { get; set; }
    }
}

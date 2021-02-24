using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApiApp.DataTransferObjects
{
    public abstract class BaseCompanyDto
    {
        public string Name { get; set; }
        public DateTime EstablishDate { get; set; }
    }

    public class CreateCompanyDto : BaseCompanyDto
    {
    }

    public class GetCompanyDto : BaseCompanyDto
    {
        public int Id { get; set; }
    }
}

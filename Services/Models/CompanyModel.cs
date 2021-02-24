using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public abstract class BaseCompanyModel
    {
        public string Name { get; set; }
        public DateTime EstablishDate { get; set; }
    }

    public class CreateCompanyModel : BaseCompanyModel
    {
    }

    public class GetCompanyModel : BaseCompanyModel
    {
        public int Id { get; set; }
    }
}

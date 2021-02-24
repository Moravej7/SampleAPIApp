using Entities;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICompanyService
    {
        Task Create(CreateCompanyModel company);
        Task<List<GetCompanyModel>> Get();
    }
}

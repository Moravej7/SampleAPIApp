using AutoMapper;
using Data;
using Entities;
using Services.Interfaces;
using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CompanyService : ICompanyService
    {
        IRepository<Company> _repository;
        IMapper _mapper;

        public CompanyService(IRepository<Company> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task Create(CreateCompanyModel companyModel)
        {
            Company company = _mapper.Map<Company>(companyModel);
            return _repository.AddAsync(company);
        }

        public async Task<List<GetCompanyModel>> Get()
        {
            List<Company> companies = await _repository.GetAsync();
            return _mapper.Map<List<GetCompanyModel>>(companies);
        }
    }
}

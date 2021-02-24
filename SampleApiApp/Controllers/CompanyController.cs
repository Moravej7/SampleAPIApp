using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleApiApp.DataTransferObjects;
using Services.Interfaces;
using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFramework;

namespace SampleApiApp.Controllers
{
    public class CompanyController : BaseAPIController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<GetCompanyModel> companyModels = await _companyService.Get();
            List<GetCompanyDto> company = _mapper.Map<List<GetCompanyDto>>(companyModels);
            return Ok(company);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateCompanyDto createCompanyDto)
        {
            CreateCompanyModel companyModel = _mapper.Map<CreateCompanyModel>(createCompanyDto);
            await _companyService.Create(companyModel);
            return Ok();
        }
    }
}

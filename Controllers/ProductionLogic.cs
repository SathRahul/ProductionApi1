using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductionApi.Model;
using ProductionApi.service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionApi.Controllers
{
    internal class ProductionLogic : AllProductionLogic
    {
        public ProductionLogic(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public Task<JsonResult> Productionreport(List<ProductionModel> model)
        {
            throw new System.NotImplementedException();
        }
    }
}
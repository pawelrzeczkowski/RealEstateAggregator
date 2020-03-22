using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgregatorM3.Web.Models;

namespace AgregatorM3.Web.Services
{
    public class SingletonDataService : ISingletonDataService
    {
        private readonly IEnumerable<IScrappingService> _scrappingServices;

        public SingletonDataService(IEnumerable<IScrappingService> scrappingServices)
        {
            _scrappingServices = scrappingServices;
        }

        public async IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel)
        {
            foreach (var service in _scrappingServices)
            {
                await foreach(var result in service.GetData(searchModel))
                {
                    yield return result;
                }
            }
        }
    }
}

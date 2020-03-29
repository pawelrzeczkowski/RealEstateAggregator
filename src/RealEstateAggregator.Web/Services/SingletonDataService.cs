using System.Collections.Generic;
using RealEstateAggregator.Web.Models;

namespace RealEstateAggregator.Web.Services
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

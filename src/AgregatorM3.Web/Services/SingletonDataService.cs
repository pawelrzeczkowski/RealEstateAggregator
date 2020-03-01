using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgregatorM3.Web.Services
{
    public class SingletonDataService : ISingletonDataService
    {
        private readonly IEnumerable<IScrappingService> _scrappingServices;

        public SingletonDataService(IEnumerable<IScrappingService> scrappingServices)
        {
            _scrappingServices = scrappingServices;
        }

        public async Task<List<string>> GetData(int priceMin, int priceMax)
        {
            var resultList = new List<string>();
            //foreach (var service in _scrappingServices)
            //{
            //    var singleResult = await service.GetData(priceMin, priceMax);
            //    resultList.AddRange(singleResult);
            //}

            var singleResult = await _scrappingServices.First().GetData(priceMin, priceMax);
            resultList.AddRange(singleResult);

            return resultList;
        }
    }
}

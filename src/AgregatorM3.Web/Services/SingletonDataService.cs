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

        public async IAsyncEnumerable<string> GetData(int priceMin, int priceMax)
        {
            foreach (var service in _scrappingServices)
            {
                await foreach(var result in service.GetData(priceMin, priceMax))
                {
                    yield return result;
                }
            }
        }
    }
}

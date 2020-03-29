using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstateAggregator.Web.Models;

namespace RealEstateAggregator.Web.Services
{
    public interface IScrappingService
    {
        IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel);
    }
}

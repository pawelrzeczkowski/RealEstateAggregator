using System.Collections.Generic;
using RealEstateAggregator.Web.Models;

namespace RealEstateAggregator.Web.Services
{
    public interface IScrappingService
    {
        IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel);
    }
}

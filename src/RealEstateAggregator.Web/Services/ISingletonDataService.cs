using System.Collections.Generic;
using RealEstateAggregator.Web.Models;

namespace RealEstateAggregator.Web.Services
{
    public interface ISingletonDataService
    {
        IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel);
    }
}

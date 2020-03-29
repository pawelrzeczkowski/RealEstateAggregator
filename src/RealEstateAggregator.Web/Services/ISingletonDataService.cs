using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateAggregator.Web.Models;

namespace RealEstateAggregator.Web.Services
{
    public interface ISingletonDataService
    {
        IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel);
    }
}

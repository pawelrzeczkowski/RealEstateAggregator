using System.Collections.Generic;
using System.Threading.Tasks;
using AgregatorM3.Web.Models;

namespace AgregatorM3.Web.Services
{
    public interface IScrappingService
    {
        IAsyncEnumerable<ResultModel> GetData(SearchModel searchModel);
    }
}

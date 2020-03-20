using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgregatorM3.Web.Models;

namespace AgregatorM3.Web.Services
{
    public interface ISingletonDataService
    {
        IAsyncEnumerable<string> GetData(SearchModel searchModel);
    }
}

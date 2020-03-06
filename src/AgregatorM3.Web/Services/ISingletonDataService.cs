using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgregatorM3.Web.Services
{
    public interface ISingletonDataService
    {
        IAsyncEnumerable<string> GetData(int priceMin, int priceMax);
    }
}

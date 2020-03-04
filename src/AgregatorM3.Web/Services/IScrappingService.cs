using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgregatorM3.Web.Services
{
    public interface IScrappingService
    {
        IAsyncEnumerable<string> GetData(int priceMin, int priceMax);
    }
}

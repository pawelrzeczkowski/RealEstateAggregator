using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgregatorM3.Web.Services
{
    public interface IScrappingService
    {
        Task<List<string>> GetData(int priceMin, int priceMax);
    }
}

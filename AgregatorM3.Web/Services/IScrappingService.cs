using AgregatorM3.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgregatorM3.Web.Services
{
    public interface IScrappingService
    {
        Task<List<string>> GetAddresses(int priceMin, int priceMax);
    }
}

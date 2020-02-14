using System.Collections.Generic;

namespace AgregatorM3.Web.Repositories
{
    public interface IOfferRepository
    {
        List<string> GetBlackList();
        List<string> GetWhiteList();
    }
}

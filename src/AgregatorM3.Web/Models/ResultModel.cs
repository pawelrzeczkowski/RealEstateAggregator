namespace AgregatorM3.Web.Services
{
    public class ResultModel
    {
        public ResultModel(string serviceName, string offerLink)
        {
            ServiceName = serviceName;
            OfferLink = offerLink;
        }
        public string ServiceName { get; private set; }
        public string OfferLink { get; private set; }
    }
}
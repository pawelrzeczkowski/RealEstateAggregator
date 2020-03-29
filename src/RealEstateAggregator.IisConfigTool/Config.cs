using System.Configuration;
using IisConfiguration.Configuration;

namespace RealEstateAggregator.IisConfigTool
{
    public class Config : EnvironmentalConfig
    {
        public string WebAppName = "RealEstateAggregator.Web";

        public int WebPort = 53099;

        public override string WebRoot
        {
            get
            {
                var root = ConfigurationManager.AppSettings["WebRoot"];
                return ReplaceSrcToken(root);
            }
        }
    }
}

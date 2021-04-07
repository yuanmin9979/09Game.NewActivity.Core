
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Api.Configuration
{
    public class BillConfig
    {
        public string Url { get; set; }

        public int AppId { get; set; }

        public int KeyVersion { get; set; }

        public string AppKey { get; set; }

        public string BillPayUrl { get; set; }

    }

    public static class ItemApiConfigService
    {
        public static IServiceCollection AddBillConfig(this IServiceCollection services, IConfiguration section)
        {
            return services.Configure<BillConfig>(section);
        }
    }
}

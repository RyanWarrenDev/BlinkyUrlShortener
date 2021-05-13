using Blinky.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blinky.Extensions
{
    public static class ContextServiceExtension
    {
        public static void AddBlinkyContext(this IServiceCollection services, string databasePath)
        {
            services.AddTransient<BlinkyContext, BlinkyContext>();
            services.Configure<LiteDbConfig>(options => options.Path = databasePath);
        }
    }
}

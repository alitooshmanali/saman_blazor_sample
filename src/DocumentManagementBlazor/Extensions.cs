using DocumentManagementBlazor;
using Serilog;

namespace DocumentManagementBlazor
{
    public static class Extensions
    {
        public static void AddLogging(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            serviceCollection.AddLogging(i =>
            {
                i.ClearProviders();
                i.AddSerilog(logger);
            });
        }
    }

}

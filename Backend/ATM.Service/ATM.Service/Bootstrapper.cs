using ATM.Service.Services;

namespace ATM.Service
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IATMService, ATMService>();

            return services;
        }
    }
}

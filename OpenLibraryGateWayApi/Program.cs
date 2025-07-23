using OpenLibraryGateWayApi.Services;
using OpenLibraryGateWayApi.AppConfig;

namespace OpenLibraryGateWayApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IContactOpenLibraryService, ContactOpenLibraryService>();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddControllers();

            var app = builder.Build();
            var setup = new SetupApp();
            setup.Config(app);

            app.Run();
        }
    }
}

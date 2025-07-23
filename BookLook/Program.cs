using BookLook.Classes;
using BookLook.Services;

namespace BookLook;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.AddServiceDefaults();

        var services = builder.Services;
        // Add services to the container.

        builder.Services.AddHttpClient<IGatewayApiService, GatewayApiService>();
        builder.Services.AddRazorPages();
        builder.Services.Configure<AppConfigOptions>(builder.Configuration.GetSection(AppConfigOptions.AppConfig));

        var app = builder.Build();

        //app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}

using Scalar.AspNetCore;
namespace OpenLibraryGateWayApi.AppConfig
{
    public class SetupApp
    {
        public void Config(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseExceptionHandler("/Error");
                app.UseHsts();

                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapControllers();
        }
    }
}

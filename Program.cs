using Serilog;
namespace Opcion1LosBorbotones.Presentation.Executors;

public class Program
{
    public static async Task Main(string[] _)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("Logger/Logs/LibrarySystemLog.txt", rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        try
        {
            var appFacade = new ApplicationFacade();
            var mainExecutor = await appFacade.CreateAppAsync();
            await mainExecutor.Execute();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during application execution");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
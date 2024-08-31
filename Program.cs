namespace Opcion1LosBorbotones.Presentation.Executors;

public class Program
{
    public static async Task Main(string[] _)
    {
        var appFacade = new ApplicationFacade();
        var mainExecutor = await appFacade.CreateAppAsync();
        await mainExecutor.Execute();
    }
}
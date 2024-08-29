using Opcion1LosBorbotones.Presentation;


public class Program
{
    public static async Task Main(string[] args)
    {
        var app = new MainMenu();
        await app.InitializeApp();
    }
}
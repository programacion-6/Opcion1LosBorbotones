using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Renders;

public static class AppPartialsRenderer
{
    public static void RenderHeader()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Los Borbotones Library")
                .Centered()
                .Color(Color.Aqua));

        var panel = new Panel("[green]Samuel Escalera \n Diego Figuera \n Bianca Soliz[/]");
        panel.Header = new PanelHeader("BY");
        AnsiConsole.Write(panel);
    }

    public static void RenderConfirmationToContinue()
    {
        AnsiConsole.Markup("[blue]Press Enter to continue.[/]");
        Console.ReadLine();
    }
}
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Utils;

public class Header
{
    public static void AppHeader()
    {
        AnsiConsole.Write(
            new FigletText("Los Borbotones Library")
                .Centered()
                .Color(Color.Aqua));
        
        var panel = new Panel("[green]Samuel Escalera \n Diego Figuera[/]");
        panel.Header = new PanelHeader("BY");
        AnsiConsole.Write(panel);
    }
}
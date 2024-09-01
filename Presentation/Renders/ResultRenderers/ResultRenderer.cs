using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Renders;

public static class ResultRenderer
{

    public static void RenderResult<T>(T? result, Func<T, string> formatterFunc)
    {
        if (result is not null)
        {
            try
            {
                var formattedResult = formatterFunc(result);
                var panel = new Panel(new Markup($"[bold green]{formattedResult}[/]"))
                {
                    Border = BoxBorder.Rounded,
                };
                AnsiConsole.Write(panel);
            }
            catch (Exception ex)
            {
                ConsoleMessageRenderer.RenderErrorMessage($"Error: {ex.Message}");
            }
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No result found");
        }
    }

    public static void RenderResults<T>(List<T> results, Func<T, string> formatterFunc)
    {
        if (results.Any())
        {
            int index = 0;
            foreach (var result in results)
            {
                var formattedResult = formatterFunc(result);

                try
                {
                    AnsiConsole.MarkupLine($"[bold]{++index}[/]. {formattedResult}");
                }
                catch (InvalidOperationException ex)
                {
                    ConsoleMessageRenderer.RenderErrorMessage($"Error in formatting result: {ex.Message}");
                }
            }
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No results found");
        }
    }

    public static void RenderResultWith<R, S>(R? result, S someElse)
    {
        if (result is not null)
        {
            var panel = new Panel(new Markup($"[bold green]{result}[/] : [bold cyan]{someElse}[/]"))
            {
                Border = BoxBorder.Rounded
            };
            AnsiConsole.Write(panel);
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No result found");

        }
    }

    public static void RenderResultWithListOf<R, S>(R? result, List<S> someElse)
    {
        if (result is not null)
        {
            var panel = new Panel(new Markup($"[bold green]{result}[/]"))
            {
                Border = BoxBorder.Rounded,
            };
            AnsiConsole.Write(panel);

            if (someElse.Any())
            {
                var list = new List<string>();
                foreach (var item in someElse)
                {
                    list.Add($"- {item}");
                }

                var listPanel = new Panel(new Markup(string.Join("\n", list)))
                {
                    Border = BoxBorder.Rounded,
                    Header = new PanelHeader("[bold]Details[/]")
                };
                AnsiConsole.Write(listPanel);
            }
            else
            {
                ConsoleMessageRenderer.RenderInfoMessage("No additional items found");
            }
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No result found");

        }
    }
}

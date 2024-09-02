using Opcion1LosBorbotones.Domain.Validator.Exceptions;
using Opcion1LosBorbotones.Logger.LogManagement;
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
                ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, ex.Message, ex);
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
                    ErrorLogger.LogErrorBasedOnSeverity(SeverityLevel.High, ex.Message, ex);
                    ConsoleMessageRenderer.RenderErrorMessage($"Error in formatting result: {ex.Message}");
                }
            }
        }
        else
        {
            ConsoleMessageRenderer.RenderInfoMessage("No results found");
        }
    }
}

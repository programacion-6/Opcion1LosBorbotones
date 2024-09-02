using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Renders;

public static class ConsoleMessageRenderer
{
    public static void RenderErrorMessage(string message)
    {
        var noFoundMessage = ConsoleMessageFormatter.AsAnError(message);
        AnsiConsole.MarkupLine(noFoundMessage);
    }

    public static void RenderHighlightMessage(string message)
    {
        var highlightMessage = ConsoleMessageFormatter.AsAHighlight(message);
        AnsiConsole.MarkupLine(highlightMessage);
    }

    public static void RenderIndicatorMessage(string message)
    {
        var indicatorMessage = ConsoleMessageFormatter.AsIndicator(message);
        AnsiConsole.MarkupLine(indicatorMessage);
    }

    public static void RenderInfoMessage(string message)
    {
        var infoMessage = ConsoleMessageFormatter.AsAnInfo(message);
        AnsiConsole.MarkupLine(infoMessage);
    }

    public static void RenderSuccessMessage(string message)
    {
        var successMessage = ConsoleMessageFormatter.AsSuccess(message);
        AnsiConsole.MarkupLine(successMessage);
    }
}
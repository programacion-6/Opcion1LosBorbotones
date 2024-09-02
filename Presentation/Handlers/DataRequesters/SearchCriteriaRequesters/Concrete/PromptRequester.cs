using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Handlers;

public class PromptRequester<I> : ISearchCriteriaRequester<I>
{
    private string _prompt;

    public PromptRequester(string prompt)
    {
        _prompt = prompt;
    }

    public I RequestCriteria()
    {
        var criteria = AnsiConsole.Ask<I>($"[bold] {_prompt}: [/]");
        return criteria;
    }
}
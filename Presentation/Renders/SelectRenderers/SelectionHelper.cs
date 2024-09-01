using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Spectre.Console;

namespace Opcion1LosBorbotones.Presentation.Renders;
public static class SelectionHelper<T> where T : IEntity
{
    public static async Task<T?> SelectItemAsync(IRepository<T> repository,
                                                string title,
                                                string emptyMessage,
                                                Func<T, string> converter)
    {
        var items = (await repository.GetAll()).ToArray();

        if (items == null || items.Length == 0)
        {
            AnsiConsole.MarkupLine($"[red]{emptyMessage}[/]");
            return default;
        }

        return AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title(title)
                .PageSize(10)
                .MoreChoicesText("[grey](Scroll up and down to see more options)[/]")
                .AddChoices(items)
                .UseConverter(converter)
        );
    }
}

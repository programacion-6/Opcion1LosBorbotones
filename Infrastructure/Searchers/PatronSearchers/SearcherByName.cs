using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Searchers;

public class SearcherByName : ISearchStrategy<Patron, string>
{
    private IPatronRepository _patronRepository;

    public SearcherByName(IPatronRepository patronRepository)
    {
        _patronRepository = patronRepository;
    }

    public string GetPrompt()
    {
        return "Patron name";
    }

    public async Task<List<Patron>> SearchByPage(string criteria, int pageNumber, int pageSize)
    {
        var patrons = await _patronRepository.GetPatronsByNameAsync(criteria, pageNumber, pageSize);
        return patrons.ToList();
    }
}
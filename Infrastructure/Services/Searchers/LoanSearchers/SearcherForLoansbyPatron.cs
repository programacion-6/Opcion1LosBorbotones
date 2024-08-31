using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searchers.LoanSearchers;

public class SearcherForLoansbyPatron : ISearchStrategy<Borrow, Guid>
{
    private readonly IBorrowRepository _repository;

    public SearcherForLoansbyPatron(IBorrowRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Borrow>> SearchByPage(Guid criteria, int pageNumber, int pageSize)
    {
        var borrows = await _repository.GetBorrowsByPatron(criteria, pageNumber, pageSize);
        return borrows.ToList();
    }
}
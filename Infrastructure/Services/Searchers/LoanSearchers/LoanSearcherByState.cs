using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searchers.LoanSearchers;

public class LoanSearcherByState : ISearchStrategy<Borrow, BorrowStatus>
{
    private readonly IBorrowRepository _repository;

    public LoanSearcherByState(IBorrowRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Borrow>> SearchByPage(BorrowStatus criteria, int pageNumber, int pageSize)
    {
        var loans = await _repository.GetBorrowsByStatus(criteria, pageNumber, pageSize);
        return loans.ToList();
    }
}
using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface ISearcher : ICommand
{
    IEnumerable<Patron> SearchByName(string searchString);
    Task<Patron?> SearchByMembershipNumber(long searchLong);
}
using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface ISearcher
{
    IEnumerable<Patron> SearchPatronByName(string searchString);
    Task<Patron?> SearchPatronByMembershipNumber(long searchLong);
    Task<IEnumerable<Book>> SearchBookByTile(string searchString);
    Task<IEnumerable<Book>> SearchBookByAuthor(string searchString);
    Task<Book?> SearchBookByIsbn(long searchString);
}
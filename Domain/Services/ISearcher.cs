using Opcion1LosBorbotones.Domain.Entity;

namespace Opcion1LosBorbotones.Domain.Repository;

public interface ISearcher
{
    IEnumerable<Patron> SearchPatronByName(string searchString, int offset, int limit);
    Task<Patron?> SearchPatronByMembershipNumber(long searchLong);
    Task<IEnumerable<Book>> SearchBookByTile(string searchString, int offset, int limit);
    Task<IEnumerable<Book>> SearchBookByAuthor(string searchString, int offset, int limit);
    Task<Book?> SearchBookByIsbn(long searchString);
}
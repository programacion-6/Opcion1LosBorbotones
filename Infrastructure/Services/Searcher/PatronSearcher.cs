using Opcion1LosBorbotones.Domain;
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class PatronSearcher : ISearcher
{
    private readonly PatronRepositoryImplementation _repository = new PatronRepositoryImplementation(new PatronDatasourceImplementation());

    public IEnumerable<Patron> SearchPatronByName(string searchString)
    {
        try
        {
            IEnumerable<Patron> patrons = _repository.GetPatronsByNameAsync(searchString).Result;

            if (!patrons.Any())
            {
                throw new InvalidOperationException("No patterns were found with the name provided.");
            }

            return patrons;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"An exception was made: {ex.Message}");
            return new List<Patron>();
        }
        
    }

    public async Task<Patron?> SearchPatronByMembershipNumber(long searchLong)
    {
        try
        {
            var patron = _repository.GetPatronByMembershipAsync(searchLong).Result;
            if (patron == null)
            {
                throw new InvalidOperationException("No pattern were found with the membership number provided.");
            }
            
            return patron;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public Task<IEnumerable<Book>> SearchBookByTile(string searchString)
    {
        throw new NotSupportedException("SearchBookByTile is not supported in PatronSearcher.");
    }

    public Task<IEnumerable<Book>> SearchBookByAuthor(string searchString)
    {
        throw new NotSupportedException("SearchBookByAuthor is not supported in PatronSearcher.");
    }

    public Task<Book?> SearchBookByIsbn(long searchString)
    {
        throw new NotSupportedException("SearchBookByIsbn is not supported in PatronSearcher.");
    }
}
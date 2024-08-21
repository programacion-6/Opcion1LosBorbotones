using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Repository;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

namespace Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class PatronSearcher : ISearcher
{
    private PatronRepositoryImplementation repository = new PatronRepositoryImplementation(new PatronDatasourceImplementation());
    
    public void Execute()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Patron> SearchByName(string searchString)
    {
        try
        {
            IEnumerable<Patron> patrons = repository.GetPatronsByNameAsync(searchString).Result;

            if (!patrons.Any())
            {
                throw new InvalidOperationException("No patterns were found with the name provided.");
            }

            return patrons;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"An exception was made: {ex.Message}");
            throw;
        }
        
    }

    public async Task<Patron?> SearchByMembershipNumber(long searchLong)
    {
        var patron = repository.GetPatronByMembershipAsync(searchLong).Result;
        return patron;
    }
}
using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Datasource;
using Opcion1LosBorbotones.Infrastructure.Repository;

public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new PatronRepositoryImplementation(new PatronDatasourceImplementation());
        IEnumerable<Patron> patrons = await repository.GetPatronsByNameAsync("Ava Martinez"); //CHANGUE THE NAME
        Console.WriteLine("Patrons segun el nombre:"); 
        foreach (var person in patrons)
        {
            Console.WriteLine(person); 
        }
        
        var patron = await repository.ReadAsync(new Guid("84d0e829-8010-4d20-8d6b-dbae2a49f25a")); //CHANGE THE GUID 
        Console.WriteLine("patron segun el id: " + patron);
        
        patron = await repository.GetPatronByMembershipAsync(1069058676); //CHANGE THE MEMBERSHIP
        Console.WriteLine("libro segun el membership number: " + patron);
    }
}
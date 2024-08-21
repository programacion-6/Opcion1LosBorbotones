using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Infrastructure.Services.Searcher;

public class Program
{
    public static async Task Main(string[] args)
    {
        var repository = new PatronSearcher();
        IEnumerable<Patron> patrons = repository.SearchByName("Ethan Thomas"); //CHANGUE THE NAME
        Console.WriteLine("Patrons segun el nombre:"); 
        foreach (var patron in patrons)
        {
            Console.WriteLine($"ID: {patron.Id}, Nombre: {patron.Name}, Membresía: {patron.MembershipNumber}");
        }

        Task<Patron?> patorn = repository.SearchByMembershipNumber(1759764659);
        Console.WriteLine("Patron segun membershipNumber: ");
        Console.WriteLine(patorn?.Result);
    }
}
namespace Opcion1LosBorbotones.Domain.Entity;

public class Patron : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; }
    public long MembershipNumber { get; }
    public long ContactDetails { get; set; }

    public Patron(Guid id, string name, long membershipNumber, long contactDetails)
    {
        Id = id;
        Name = name;
        MembershipNumber = membershipNumber;
        this.ContactDetails = contactDetails;
    }
}
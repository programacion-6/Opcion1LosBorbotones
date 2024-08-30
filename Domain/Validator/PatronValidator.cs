using Opcion1LosBorbotones.Domain.Entity;
using Opcion1LosBorbotones.Domain.Validator.Exceptions;
using Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;

namespace Opcion1LosBorbotones.Domain.Validator;

public class PatronValidator
{
    public void ValidatePatron(Patron patron)
    {
        if (patron == null)
        {
            throw new PatronException(
                "Patron object cannot be null",
                SeverityLevel.Critical,
                "This error occurs when the Patron object is not initialized. " +
                "Ensure that the Patron object is properly instantiated and filled with the necessary data before passing it for validation.");
        }

        ValidateName(patron.Name);
        ValidateMembershipNumber(patron.MembershipNumber);
        ValidateContactDetails(patron.ContactDetails);
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new PatronException(
                "Patron name cannot be null or empty",
                SeverityLevel.Medium,
                "The name of the patron should not be null or empty. " +
                "Please provide a valid name for the patron.");
        }

        if (name.Length > 100)
        {
            throw new PatronException(
                "Patron name exceeds maximum length",
                SeverityLevel.Medium,
                "The name of the patron should not exceed 100 characters. " +
                "Please provide a shorter name for the patron.");
        }

        if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new PatronException(
                "Patron name must contain only letters",
                SeverityLevel.Medium,
                "The name of the patron should consist of alphabetic characters only. " +
                "Please verify the patron's name and ensure it does not contain any special characters or numbers.");
        }
    }

    private void ValidateMembershipNumber(long membershipNumber)
    {
        if (membershipNumber <= 0)
        {
            throw new PatronException(
                "Membership number must be a positive integer",
                SeverityLevel.Medium,
                "The membership number of the patron should be a positive integer greater than zero. " +
                "Please provide a valid membership number for the patron.");
        }

        if (membershipNumber.ToString().Length > 10)
        {
            throw new PatronException(
                "Membership number exceeds maximum length",
                SeverityLevel.Medium,
                "The membership number of the patron should not exceed 10 digits. " +
                "Please provide a valid membership number for the patron.");
        }
    }

    private void ValidateContactDetails(long contactDetails)
    {
        if (contactDetails <= 0)
        {
            throw new PatronException(
                "Contact details must be a positive integer",
                SeverityLevel.Medium,
                "The contact details of the patron should be a positive integer greater than zero. " +
                "Please provide valid contact details for the patron.");
        }

        if (contactDetails.ToString().Length != 8)
        {
            throw new PatronException(
                "Contact details must be 8 digits long in Bolivian format",
                SeverityLevel.Medium,
                "The contact details of the patron should be 8 digits long. " +
                "Please provide a valid 8-digit contact number for the patron in Bolivian format.");
        }

        if (contactDetails < 60000000 || contactDetails > 79999999)
        {
            throw new PatronException(
                "Contact details must be in the range of Bolivian phone numbers",
                SeverityLevel.Medium,
                "The contact details of the patron should fall within the valid range for Bolivian phone numbers.");
        }
    }

}

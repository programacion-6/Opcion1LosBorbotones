namespace Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;

public class BookException : CustomException
{
    public BookException(string message, SeverityLevel severityLevel = SeverityLevel.Medium, string resolutionSuggestion = "") 
            : base(message, severityLevel, resolutionSuggestion)
    {
    }
}

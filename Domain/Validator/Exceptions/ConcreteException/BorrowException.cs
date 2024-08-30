namespace Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;

public class BorrowException : CustomException
{
    public BorrowException(string message, SeverityLevel severityLevel = SeverityLevel.Medium, string resolutionSuggestion = "") 
            : base(message, severityLevel, resolutionSuggestion)
    {
    }

}

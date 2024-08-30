namespace Opcion1LosBorbotones.Domain.Validator.Exceptions.ConcreteException;

public class PatronException : CustomException
{
    public PatronException(string message, SeverityLevel severityLevel = SeverityLevel.Medium, string resolutionSuggestion = "") 
            : base(message, severityLevel, resolutionSuggestion)
    {
    }

}

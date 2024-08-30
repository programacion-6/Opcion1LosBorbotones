namespace Opcion1LosBorbotones.Domain.Validator.Exceptions;

public class CustomException : Exception
{
    public SeverityLevel Severity { get; }
    public string ResolutionSuggestion { get; }

    public CustomException(string message, SeverityLevel severityLevel, string resolutionSuggestion = "")
        : base(message)
    {
        Severity = severityLevel;
        ResolutionSuggestion = resolutionSuggestion;
    }
}

using System.ComponentModel.DataAnnotations;

public static class ValidationHelper
{
    public static bool TryValidateModel(object model, out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        return Validator.TryValidateObject(model, validationContext, validationResults, true);
    }
}

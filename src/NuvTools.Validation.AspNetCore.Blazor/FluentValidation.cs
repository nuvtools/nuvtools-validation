using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;

namespace NuvTools.Validation.AspNetCore.Blazor;

public class FluentValidation<TModel> where TModel : class, new()
{
    private readonly IValidator<TModel> _validator;
    private readonly TModel _model;
    private readonly EditContext _editContext;
    private readonly ValidationMessageStore _validationMessageStore;

    public FluentValidation(TModel model,
            IValidator<TModel> validator,
            EditContext editContext,
            bool autoValidationOnRequested = true,
            bool autoValidationOnFieldChanged = true)
    {
        _model = model;
        _validator = validator;
        _editContext = editContext;

        _validationMessageStore = new ValidationMessageStore(_editContext);

        if (autoValidationOnRequested)
            _editContext.OnValidationRequested += (sender, args) => ValidateModelAndHighlightInvalidFields();

        if (autoValidationOnFieldChanged)
            _editContext.OnFieldChanged += (sender, args) => ValidateField(args.FieldIdentifier);
    }

    /// <summary>
    /// Perform validation for the entire model and populate the ValidationMessageStore.
    /// </summary>
    /// <returns>The validation result.</returns>
    public FluentValidation.Results.ValidationResult Validate()
    {
        var validationResult = _validator.Validate(_model);

        _validationMessageStore.Clear();

        foreach (var error in validationResult.Errors)
        {
            var fieldIdentifier = new FieldIdentifier(_model, error.PropertyName);
            _validationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
        }

        return validationResult;
    }

    /// <summary>
    /// Validate the entire model and highlight invalid fields in the UI.
    /// </summary>
    /// <returns>The validation result.</returns>
    public FluentValidation.Results.ValidationResult ValidateModelAndHighlightInvalidFields()
    {
        var validationResult = Validate();

        _editContext.NotifyValidationStateChanged();

        return validationResult;
    }

    /// <summary>
    /// Validate a specific field and update its validation state.
    /// </summary>
    /// <param name="fieldIdentifier">The field to validate.</param>
    public void ValidateField(FieldIdentifier fieldIdentifier)
    {
        var propertyName = fieldIdentifier.FieldName;

        // Create a ValidationContext for the specific field
        var validationContext = new ValidationContext<TModel>(_model)
        {
            RootContextData = { ["__FV_PropertyName__"] = propertyName }
        };

        var validationResult = _validator.Validate(validationContext);

        _validationMessageStore.Clear(fieldIdentifier);

        foreach (var error in validationResult.Errors.Where(e => e.PropertyName == propertyName))
        {
            _validationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
        }
    }
}

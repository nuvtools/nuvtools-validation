using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace NuvTools.Validation.AspNetCore.Blazor;

public class FluentValidation<TModel> where TModel : class, new()
{
    private readonly IValidator<TModel> _validator;

    private readonly TModel _model;

    private readonly EditContext _editContext;

    private readonly ValidationMessageStore _validationMessageStore;

    public FluentValidation(TModel model, IValidator<TModel> validator, EditContext editContext, bool autoValidationOnRequested = true, bool autoValidationOnFieldChanged = true, bool highlightInvalidFields = true)
    {
        _model = model;
        _validator = validator;
        _editContext = editContext;
        _validationMessageStore = new ValidationMessageStore(_editContext);
        if (autoValidationOnRequested)
        {
            _editContext.OnValidationRequested += delegate
            {
                Validate();
            };
        }

        if (autoValidationOnFieldChanged)
        {
            _editContext.OnFieldChanged += delegate (object? sender, FieldChangedEventArgs args)
            {
                ValidateField(args.FieldIdentifier);
            };
        }

        if (highlightInvalidFields)
            _editContext.NotifyValidationStateChanged();
    }

    //
    // Summary:
    //     Perform validation for the entire model and populate the ValidationMessageStore.
    //
    //
    // Returns:
    //     The validation result.
    public ValidationResult Validate(bool highlightInvalidFields = true)
    {
        ValidationResult validationResult = _validator.Validate(_model);
        _validationMessageStore.Clear();
        foreach (ValidationFailure error in validationResult.Errors)
        {
            FieldIdentifier fieldIdentifier = ToFieldIdentifier(_editContext, error.PropertyName);
            _validationMessageStore.Add(in fieldIdentifier, error.ErrorMessage);
        }

        if (highlightInvalidFields)
            _editContext.NotifyValidationStateChanged();

        return validationResult;
    }
    private static FieldIdentifier ToFieldIdentifier(EditContext editContext, string propertyPath)
    {
        var model = editContext.Model;
        var parts = propertyPath.Split('.');
        if (parts.Length == 1)
            return new FieldIdentifier(model, propertyPath);

        object? currentObject = model;
        for (int i = 0; i < parts.Length - 1; i++)
        {
            var property = currentObject?.GetType().GetProperty(parts[i]);
            currentObject = property?.GetValue(currentObject);
            if (currentObject == null)
                break;
        }

        return new FieldIdentifier(currentObject!, parts.Last());
    }

    //
    // Summary:
    //     Validate a specific field and update its validation state.
    //
    // Parameters:
    //   fieldIdentifier:
    //     The field to validate.
    public void ValidateField(FieldIdentifier fieldIdentifier, bool highlightInvalidField = true)
    {
        string propertyName = fieldIdentifier.FieldName;
        ValidationContext<TModel> validationContext = new ValidationContext<TModel>(_model);
        validationContext.RootContextData["__FV_PropertyName__"] = propertyName;
        ValidationContext<TModel> context = validationContext;
        ValidationResult validationResult = _validator.Validate(context);
        _validationMessageStore.Clear(in fieldIdentifier);
        foreach (ValidationFailure item in validationResult.Errors.Where((ValidationFailure e) => e.PropertyName == propertyName))
        {
            _validationMessageStore.Add(in fieldIdentifier, item.ErrorMessage);
        }

        if (highlightInvalidField)
            _editContext.NotifyValidationStateChanged();
    }
}
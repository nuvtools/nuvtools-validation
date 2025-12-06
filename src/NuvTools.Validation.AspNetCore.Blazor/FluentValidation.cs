using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace NuvTools.Validation.AspNetCore.Blazor;

/// <summary>
/// Integrates FluentValidation with Blazor's EditContext and ValidationMessageStore.
/// Provides automatic validation on form submission and field changes, with support for nested property paths.
/// </summary>
/// <typeparam name="TModel">The type of the model being validated.</typeparam>
public class FluentValidation<TModel> where TModel : class, new()
{
    private readonly IValidator<TModel> _validator;

    private readonly TModel _model;

    private readonly EditContext _editContext;

    private readonly ValidationMessageStore _validationMessageStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentValidation{TModel}"/> class.
    /// </summary>
    /// <param name="model">The model instance to validate.</param>
    /// <param name="validator">The FluentValidation validator for the model.</param>
    /// <param name="editContext">The Blazor EditContext to integrate with.</param>
    /// <param name="autoValidationOnRequested">If true, automatically validates the entire model when EditContext.Validate() is called. Default is true.</param>
    /// <param name="autoValidationOnFieldChanged">If true, automatically validates individual fields as they change. Default is true.</param>
    /// <param name="highlightInvalidFields">If true, notifies the EditContext of validation state changes to highlight invalid fields. Default is true.</param>
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
    /// <summary>
    /// Converts a property path string (e.g., "Address.City") into a FieldIdentifier by traversing the object graph.
    /// This enables validation errors on nested properties to bind correctly in Blazor forms.
    /// </summary>
    /// <param name="editContext">The EditContext containing the root model.</param>
    /// <param name="propertyPath">The dot-separated property path (e.g., "Address.City").</param>
    /// <returns>A FieldIdentifier representing the target property.</returns>
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

    /// <summary>
    /// Validates a specific field and updates its validation state in the EditContext.
    /// </summary>
    /// <param name="fieldIdentifier">The field to validate.</param>
    /// <param name="highlightInvalidField">If true, notifies the EditContext to highlight the field if invalid. Default is true.</param>
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
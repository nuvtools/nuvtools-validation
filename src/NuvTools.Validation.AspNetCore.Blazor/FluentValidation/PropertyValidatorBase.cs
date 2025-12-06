using FluentValidation;

namespace NuvTools.Validation.AspNetCore.Blazor.FluentValidation;

/// <summary>
/// Base abstract validator that provides property-level validation support
/// using FluentValidation, specifically designed for MudBlazor MudForm integration.
/// </summary>
/// <remarks>
/// MudBlazor's MudForm component expects a validation delegate with the signature
/// <c>Func&lt;object, string, Task&lt;IEnumerable&lt;string&gt;&gt;&gt;</c>.
/// This base class exposes the <see cref="ValidatePropertyAsync"/> property that
/// matches this signature, enabling seamless integration with MudForm's Validation parameter.
/// </remarks>
/// <typeparam name="T">
/// The type of the model being validated.
/// </typeparam>
/// <example>
/// <code>
/// public class MyModelValidator : PropertyValidatorBase&lt;MyModel&gt;
/// {
///     public MyModelValidator()
///     {
///         RuleFor(x => x.Email).NotEmpty().EmailAddress();
///     }
/// }
///
/// // In Blazor component:
/// &lt;MudForm Model="@model" Validation="@(validator.ValidatePropertyAsync)"&gt;
///     &lt;MudTextField @bind-Value="model.Email" For="@(() => model.Email)" /&gt;
/// &lt;/MudForm&gt;
/// </code>
/// </example>
public abstract class PropertyValidatorBase<T> : AbstractValidator<T>
{
    /// <summary>
    /// Gets a delegate that validates a single property asynchronously.
    /// This delegate is designed to be passed directly to MudBlazor's MudForm Validation parameter.
    /// </summary>
    /// <remarks>
    /// MudForm calls this delegate automatically for each field during validation.
    /// Only the specified property will be evaluated by FluentValidation.
    /// </remarks>
    /// <value>
    /// A delegate that takes a model instance and a property name, and returns a task containing a sequence of validation error messages.
    /// Returns an empty collection when the property is valid.
    /// </value>
    public Func<object, string, Task<IEnumerable<string>>> ValidatePropertyAsync =>
        async (model, propertyName) =>
        {
            var result = await ValidateAsync(
                ValidationContext<T>.CreateWithOptions(
                    (T)model,
                    x => x.IncludeProperties(propertyName)
                )
            );

            return result.IsValid
                ? []
                : result.Errors.Select(e => e.ErrorMessage);
        };
}

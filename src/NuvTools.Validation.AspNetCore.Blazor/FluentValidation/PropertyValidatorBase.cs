using FluentValidation;

namespace NuvTools.Validation.AspNetCore.Blazor.FluentValidation;

/// <summary>
/// Base abstract validator that provides property-level validation support
/// using FluentValidation.
/// </summary>
/// <typeparam name="T">
/// The type of the model being validated.
/// </typeparam>
public abstract class PropertyValidatorBase<T> : AbstractValidator<T>
{
    /// <summary>
    /// Validates a single property of the provided model asynchronously.
    /// Only the specified property will be evaluated by FluentValidation.
    /// </summary>
    /// <remarks>
    /// This is useful when validating individual fields dynamically,
    /// such as in UI scenarios where validation must occur on focus-change
    /// or as a user types.
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

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using NuvTools.Validation.AspNetCore.Blazor;
using NUnit.Framework;
using System.Linq;

namespace NuvTools.Validation.Tests;

[TestFixture]
public class FluentValidationTests
{
    [Test]
    public void Validate_ModelWithErrors_ReturnsValidationResult()
    {
        // Arrange
        var model = new TestModel(); // Name and City are empty
        var validator = new TestModelValidator();
        var editContext = new EditContext(model);
        var fv = new FluentValidation<TestModel>(model, validator, editContext);

        // Act
        ValidationResult result = fv.Validate();

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(2));
        Assert.That(result.Errors.Any(e => e.PropertyName == "Name"));
        Assert.That(result.Errors.Any(e => e.PropertyName == "Address.City"));
    }

    [Test]
    public void ValidateField_SpecificField_ShowsOnlyThatError()
    {
        // Arrange
        var model = new TestModel(); // Name is empty
        var validator = new TestModelValidator();
        var editContext = new EditContext(model);
        var fv = new FluentValidation<TestModel>(model, validator, editContext);
        var fieldIdentifier = new FieldIdentifier(model, nameof(TestModel.Name));

        // Act
        fv.ValidateField(fieldIdentifier);

        // Assert — only Name field should have validation error
        var messages = editContext.GetValidationMessages(fieldIdentifier).ToList();
        Assert.That(messages, Has.Count.EqualTo(1));
        Assert.That(messages[0], Does.Contain("Name is required"));
    }

    [Test]
    public void Validate_NestedProperty_BindsToCorrectFieldIdentifier()
    {
        // Arrange
        var model = new TestModel(); // Address.City is empty
        var validator = new TestModelValidator();
        var editContext = new EditContext(model);
        var fv = new FluentValidation<TestModel>(model, validator, editContext);

        // Act
        var result = fv.Validate();

        // Assert
        var nestedField = new FieldIdentifier(model.Address, nameof(Address.City));
        var messages = editContext.GetValidationMessages(nestedField).ToList();
        Assert.That(messages, Has.Count.EqualTo(1));
        Assert.That(messages[0], Does.Contain("City is required"));
    }

    [Test]
    public void AutoValidationEvents_ShouldTriggerOnEditContext()
    {
        // Arrange
        var model = new TestModel();
        var validator = new TestModelValidator();
        var editContext = new EditContext(model);
        var fv = new FluentValidation<TestModel>(model, validator, editContext);

        // Act
        var triggered = false;
        editContext.OnValidationStateChanged += (_, _) => triggered = true;

        editContext.Validate();

        // Assert
        Assert.That(triggered, Is.True);
    }

    public class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
    }

    public class Address
    {
        public string City { get; set; } = string.Empty;
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Address.City).NotEmpty().WithMessage("City is required");
        }
    }
}

# NuvTools Validation Library

[![NuGet](https://img.shields.io/nuget/v/NuvTools.Validation.svg)](https://www.nuget.org/packages/NuvTools.Validation/)
[![License](https://img.shields.io/github/license/nuvtools/nuvtools-validation)](LICENSE)

NuvTools Validation is a comprehensive validation library for .NET 8, 9, and 10, designed to provide robust validation features for Web, Desktop, and Mobile (MAUI) applications. It includes specialized validators for Brazilian documents, password complexity rules, and seamless integration with Blazor and FluentValidation.

## 📦 Packages

| Package | Version | Description |
|---------|---------|-------------|
| **NuvTools.Validation** | [![NuGet](https://img.shields.io/nuget/v/NuvTools.Validation.svg)](https://www.nuget.org/packages/NuvTools.Validation/) | Core validation library with Brazilian validators, regex patterns, and data annotations |
| **NuvTools.Validation.AspNetCore.Blazor** | [![NuGet](https://img.shields.io/nuget/v/NuvTools.Validation.AspNetCore.Blazor.svg)](https://www.nuget.org/packages/NuvTools.Validation.AspNetCore.Blazor/) | Blazor integration with FluentValidation support |

## 🚀 Installation

Install via NuGet Package Manager:

```bash
dotnet add package NuvTools.Validation
```

For Blazor applications with FluentValidation:

```bash
dotnet add package NuvTools.Validation.AspNetCore.Blazor
```

## ✨ Features

### Core Validation (NuvTools.Validation)

- **Brazilian Document Validators**
  - CPF (Cadastro de Pessoas Físicas) validation
  - CNPJ (Cadastro Nacional da Pessoa Jurídica) validation
  - Brazilian mobile phone number validation
  - ZIP code (CEP) validation
  - Auto-detection of CPF or CNPJ

- **General Validators**
  - Email address validation
  - Numeric validation (int, long, decimal)
  - Base64 content and Data URI validation

- **Data Annotations**
  - CPF/CNPJ validation attributes
  - Password complexity attributes (capital letters, lowercase, digits)
  - Custom validation attributes

- **Formatting Utilities**
  - CPF formatting (XXX.XXX.XXX-XX)
  - CNPJ formatting (XX.XXX.XXX/XXXX-XX)

### Blazor Integration (NuvTools.Validation.AspNetCore.Blazor)

- FluentValidation integration with EditContext
- MudBlazor MudForm integration via PropertyValidatorBase
- Automatic validation on form submission
- Field-level validation on change
- Support for nested property validation
- ValidationMessageStore integration

## 📖 Usage Examples

### Brazilian Document Validation

```csharp
using NuvTools.Validation.Brazil;

// Validate CPF
string cpf = "123.456.789-01";
bool isValid = cpf.IsCPF(); // Returns true or false

// Validate CNPJ
string cnpj = "12.345.678/0001-95";
bool isValid = cnpj.IsCNPJ(); // Returns true or false

// Auto-detect CPF or CNPJ
string document = "12345678901";
bool isValid = document.IsCPForCNPJ(); // Validates based on length

// Validate mobile phone
string phone = "11987654321";
bool isValid = phone.IsMobileNumber(); // Validates Brazilian mobile format

// Validate ZIP code (CEP)
string zipCode = "01310-100";
bool isValid = zipCode.IsZipCodeNumber(); // Validates format
```

### Document Formatting

```csharp
using NuvTools.Validation.Brazil;

// Format CPF
string cpf = "12345678901";
string formatted = cpf.FormatCPF(); // Returns "123.456.789-01"

// Format CNPJ
string cnpj = "12345678000195";
string formatted = cnpj.FormatCNPJ(); // Returns "12.345.678/0001-95"
```

### General Validation

```csharp
using NuvTools.Validation;

// Email validation
string email = "user@example.com";
bool isValid = email.IsEmail();

// Numeric validation
string number = "12345";
bool isInt = number.IsIntNumber();
bool isPositive = number.IsIntNumber(positiveOnly: true);
bool isLong = number.IsLongNumber();

// Decimal validation
string decimal = "123.45";
bool isDecimal = decimal.IsDecimalNumber();
```

### Data Annotations

```csharp
using NuvTools.Validation.Brazil.Annotations;
using NuvTools.Validation.Annotations;

public class UserRegistrationModel
{
    [Required]
    [CPF]
    public string CPF { get; set; }

    [Required]
    [PasswordComplexityCapitalLetters(2)]
    [PasswordComplexityLowerCaseLetters(2)]
    [PasswordComplexityDigits(2)]
    public string Password { get; set; }
}

public class CompanyModel
{
    [Required]
    [CNPJ]
    public string CNPJ { get; set; }
}

// Validate using data annotations
var model = new UserRegistrationModel
{
    CPF = "123.456.789-01",
    Password = "MyPass123"
};

var errors = model.Validate(); // Returns null if valid, or list of error messages
```

### Blazor FluentValidation Integration

```csharp
@page "/register"
@using NuvTools.Validation.AspNetCore.Blazor
@using FluentValidation

<EditForm Model="@model" OnValidSubmit="HandleSubmit">
    <NuvTools.Validation.AspNetCore.Blazor.FluentValidation TModel="UserModel"
        @ref="fluentValidation"
        Validator="validator"
        Model="model"
        EditContext="editContext" />

    <InputText @bind-Value="model.Name" />
    <ValidationMessage For="() => model.Name" />

    <InputText @bind-Value="model.Address.City" />
    <ValidationMessage For="() => model.Address.City" />

    <button type="submit">Submit</button>
</EditForm>

@code {
    private UserModel model = new();
    private EditContext editContext;
    private UserModelValidator validator = new();
    private FluentValidation<UserModel> fluentValidation;

    protected override void OnInitialized()
    {
        editContext = new EditContext(model);
        fluentValidation = new FluentValidation<UserModel>(
            model,
            validator,
            editContext,
            autoValidationOnRequested: true,
            autoValidationOnFieldChanged: true
        );
    }

    public class UserModel
    {
        public string Name { get; set; }
        public Address Address { get; set; } = new();
    }

    public class Address
    {
        public string City { get; set; }
    }

    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address.City).NotEmpty();
        }
    }

    private void HandleSubmit()
    {
        // Form is valid
    }
}
```

### MudBlazor Integration with PropertyValidatorBase

The `PropertyValidatorBase` class is specifically designed for use with [MudBlazor](https://mudblazor.com/) forms. MudForm components automatically call the `ValidatePropertyAsync` delegate for field-level validation.

```csharp
@page "/register"
@using MudBlazor
@using NuvTools.Validation.AspNetCore.Blazor.FluentValidation
@using FluentValidation

<MudForm Model="@model" @ref="form" Validation="@(validator.ValidatePropertyAsync)">
    <MudTextField @bind-Value="model.Email"
                  For="@(() => model.Email)"
                  Label="Email"
                  Required="true" />

    <MudTextField @bind-Value="model.Name"
                  For="@(() => model.Name)"
                  Label="Name"
                  Required="true" />

    <MudTextField @bind-Value="model.Password"
                  For="@(() => model.Password)"
                  Label="Password"
                  InputType="InputType.Password"
                  Required="true" />

    <MudButton Variant="Variant.Filled"
               Color="Color.Primary"
               OnClick="@(async () => await Submit())">
        Register
    </MudButton>
</MudForm>

@code {
    private MudForm form;
    private UserModel model = new();
    private UserModelValidator validator = new();

    public class UserModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class UserModelValidator : PropertyValidatorBase<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        }
    }

    private async Task Submit()
    {
        await form.Validate();

        if (form.IsValid)
        {
            // Process the valid form
        }
    }
}
```

## 🎯 Supported Frameworks

- .NET 8
- .NET 9
- .NET 10

## 🔧 Building from Source

```bash
# Clone the repository
git clone https://github.com/nuvtools/nuvtools-validation.git
cd nuvtools-validation

# Build the solution
dotnet build NuvTools.Validation.slnx -c Release

# Run tests
dotnet test NuvTools.Validation.slnx -c Release

# Create NuGet packages
dotnet pack NuvTools.Validation.slnx -c Release
```

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 🔗 Links

- [GitHub Repository](https://github.com/nuvtools/nuvtools-validation)
- [NuGet Package - NuvTools.Validation](https://www.nuget.org/packages/NuvTools.Validation/)
- [NuGet Package - NuvTools.Validation.AspNetCore.Blazor](https://www.nuget.org/packages/NuvTools.Validation.AspNetCore.Blazor/)
- [Website](https://nuvtools.com)

## 💡 Support

If you encounter any issues or have questions, please [open an issue](https://github.com/nuvtools/nuvtools-validation/issues) on GitHub.

---

Copyright © 2025 Nuv Tools
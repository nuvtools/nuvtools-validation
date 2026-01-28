# NuvTools Validation Libraries

[![NuGet](https://img.shields.io/nuget/v/NuvTools.Validation.svg)](https://www.nuget.org/packages/NuvTools.Validation/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A comprehensive validation library for .NET applications, providing robust validation features for Web, Desktop, and Mobile (MAUI) applications. Includes specialized validators for Brazilian documents, password complexity rules, and seamless integration with Blazor and FluentValidation. These libraries target modern .NET platforms, including .NET 8, .NET 9, and .NET 10.

## Libraries

### NuvTools.Validation

Core validation library with Brazilian document validators, regex patterns, and data annotations.

**Key Features:**
- **Brazilian Document Validators**: CPF, CNPJ, mobile phone, and ZIP code (CEP) validation
- **General Validators**: Email, numeric types (int, long, decimal), Base64/Data URI validation
- **Data Annotations**: CPF/CNPJ attributes, password complexity attributes
- **Formatting Utilities**: CPF and CNPJ formatting helpers

### NuvTools.Validation.AspNetCore.Blazor

Blazor integration for FluentValidation with EditContext and ValidationMessageStore support.

**Key Features:**
- **FluentValidation Integration**: Bridges FluentValidation with Blazor's EditContext
- **Auto-Validation**: Automatic validation on form submission and field change
- **Nested Properties**: Support for nested property path validation
- **Field Highlighting**: Configurable invalid field highlighting via ValidationMessageStore

## Installation

Install via NuGet Package Manager:

```bash
# For core validation features
dotnet add package NuvTools.Validation

# For Blazor applications with FluentValidation
dotnet add package NuvTools.Validation.AspNetCore.Blazor
```

Or via Package Manager Console:

```powershell
Install-Package NuvTools.Validation
Install-Package NuvTools.Validation.AspNetCore.Blazor
```

## Quick Start

### Brazilian Document Validation

```csharp
using NuvTools.Validation.Brazil;

// Validate CPF
string cpf = "123.456.789-01";
bool isValid = cpf.IsCPF();

// Validate CNPJ
string cnpj = "12.345.678/0001-95";
bool isValid = cnpj.IsCNPJ();

// Auto-detect CPF or CNPJ
string document = "12345678901";
bool isValid = document.IsCPForCNPJ();

// Validate mobile phone
string phone = "11987654321";
bool isValid = phone.IsMobileNumber();

// Validate ZIP code (CEP)
string zipCode = "01310-100";
bool isValid = zipCode.IsZipCodeNumber();
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
string value = "123.45";
bool isDecimal = value.IsDecimalNumber();
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

```razor
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

## Features

- **Multi-targeting**: Compatible with .NET 8, .NET 9, and .NET 10
- **Comprehensive documentation**: Full XML documentation for IntelliSense
- **Modular design**: Use only what you need
- **Modern C# features**: Uses nullable reference types, implicit usings, and source-generated regex

## Building from Source

This project uses the modern `.slnx` solution format (Visual Studio 2022 v17.11+).

```bash
# Clone the repository
git clone https://github.com/nuvtools/nuvtools-validation.git
cd nuvtools-validation

# Build the solution
dotnet build NuvTools.Validation.slnx

# Run tests
dotnet test NuvTools.Validation.slnx

# Create release packages
dotnet build NuvTools.Validation.slnx --configuration Release
```

## Requirements

- .NET 8.0 SDK or higher
- Visual Studio 2022 (v17.11+) or Visual Studio Code with C# extension
- FluentValidation 12.x (for NuvTools.Validation.AspNetCore.Blazor)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [GitHub Repository](https://github.com/nuvtools/nuvtools-validation)
- [NuGet Package - NuvTools.Validation](https://www.nuget.org/packages/NuvTools.Validation/)
- [NuGet Package - NuvTools.Validation.AspNetCore.Blazor](https://www.nuget.org/packages/NuvTools.Validation.AspNetCore.Blazor/)
- [Official Website](https://nuvtools.com)

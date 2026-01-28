# NuvTools Validation Library

[![NuGet](https://img.shields.io/nuget/v/NuvTools.Validation.svg)](https://www.nuget.org/packages/NuvTools.Validation/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A validation library for .NET applications, providing robust validation features for Web, Desktop, and Mobile (MAUI) applications. Includes email and numeric validators, specialized Brazilian document validators (CPF, CNPJ, phone, CEP), password complexity attributes, and data annotations. This library targets modern .NET platforms, including .NET 8, .NET 9, and .NET 10.

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package NuvTools.Validation
```

Or via Package Manager Console:

```powershell
Install-Package NuvTools.Validation
```

## Key Features

- **General Validators**: Email, numeric types (int, long, decimal), Base64/Data URI validation
- **Brazilian Document Validators**: CPF, CNPJ, mobile phone, and ZIP code (CEP) validation
- **Document Formatting**: CPF and CNPJ formatting helpers
- **Data Annotations**: CPF/CNPJ validation attributes, password complexity attributes
- **Localized Messages**: Validation messages in English and Portuguese (pt-BR)

## Quick Start

### General Validation

```csharp
using NuvTools.Validation;

// Email validation
bool isValid = "user@example.com".IsEmail();

// Numeric validation
bool isInt = "12345".IsIntNumber();
bool isPositive = "12345".IsIntNumber(positiveOnly: true);
bool isLong = "12345".IsLongNumber();

// Decimal validation
bool isDecimal = "123.45".IsDecimalNumber();
```

### Brazilian Document Validation

```csharp
using NuvTools.Validation.Brazil;

// Validate CPF
bool isValid = "123.456.789-01".IsCPF();

// Validate CNPJ
bool isValid = "12.345.678/0001-95".IsCNPJ();

// Auto-detect CPF or CNPJ
bool isValid = "12345678901".IsCPForCNPJ();

// Validate mobile phone
bool isValid = "11987654321".IsMobileNumber();

// Validate ZIP code (CEP)
bool isValid = "01310-100".IsZipCodeNumber();
```

### Document Formatting

```csharp
using NuvTools.Validation.Brazil;

// Format CPF
string formatted = "12345678901".FormatCPF();
// Output: 123.456.789-01

// Format CNPJ
string formatted = "12345678000195".FormatCNPJ();
// Output: 12.345.678/0001-95
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

## Features

- **Multi-targeting**: Compatible with .NET 8, .NET 9, and .NET 10
- **Comprehensive documentation**: Full XML documentation for IntelliSense
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

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [GitHub Repository](https://github.com/nuvtools/nuvtools-validation)
- [NuGet Package - NuvTools.Validation](https://www.nuget.org/packages/NuvTools.Validation/)
- [Official Website](https://nuvtools.com)

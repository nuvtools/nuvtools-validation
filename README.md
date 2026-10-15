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
- **Brazilian Document Validators**: CPF, CNPJ (including the new RFB alphanumeric format), mobile phone, and ZIP code (CEP) validation
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

// Validate CNPJ (numeric)
bool isValid = "12.345.678/0001-95".IsCNPJ();

// Validate CNPJ (new RFB alphanumeric format — uppercase A–Z and 0–9 in positions 1–12)
bool isValidAlpha = "12.ABC.345/01DE-35".IsCNPJ();

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

## Releasing

Publishing to nuget.org is irreversible per version, so the workflow makes it the last and smallest
step.

The package has its own workflow, and the steps live in the reusable `package-publish.yml`, the
same layout as the other NuvTools repositories.

| Package | Workflow | Release tag |
|---|---|---|
| `NuvTools.Validation` | `validation-publish.yml` | `validation-vX.Y.Z` |

1. Set the package's `<Version>` in its `.csproj` and merge it. That is the version that gets
   published; the workflow reads it and never computes one.
2. Run the library's workflow by hand (*Actions → Validation Publish → Run workflow*) with *push*
   checked.
3. The workflow builds the solution with `-warnaserror`, runs the tests, takes **that package only**
   from the build output, verifies it carries its README, licence, icon, symbols and every target
   framework, and then waits for approval on the `nuget` GitHub Environment before pushing.
4. **After the push succeeds, the workflow tags the commit** with the tag in the table above. The tag is a
   record of what is on nuget.org, never what starts a release, so it cannot exist for a version
   that did not get there. A run whose tag already exists stops before building: the version has
   been released and `<Version>` needs a bump.

**Versions published by hand are refused.** Releases made before these workflows exist on nuget.org
with no tag. Pushing one again would skip the duplicate and then tag the current commit as a
release it is not, so a version that is already on nuget.org stops the run. The exception is a
re-run of the same run, which is how a push that succeeded and a tag that failed gets finished.

**No API key is stored.** The push uses nuget.org's trusted publishing: the job presents a GitHub
OIDC token, nuget.org checks it against the policy registered for this repository and answers with a
key that lives for one hour. The repository needs one secret, `NUGET_USER` — the nuget.org profile
name the policy belongs to — and the policy itself, registered under *Trusted Publishing* on
nuget.org with the owner `nuvtools`, the repository `nuvtools-validation`, the workflow file and the
environment `nuget`.

Run a library's workflow by hand with *push* unchecked for a dry run that stops after the
verification.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [GitHub Repository](https://github.com/nuvtools/nuvtools-validation)
- [NuGet Package - NuvTools.Validation](https://www.nuget.org/packages/NuvTools.Validation/)
- [Official Website](https://nuvtools.com)

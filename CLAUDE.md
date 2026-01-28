# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NuvTools.Validation is a multi-targeted .NET validation library designed for Web, Desktop, and Mobile (MAUI) applications. The solution consists of two libraries:

- **NuvTools.Validation** - Core validation library with email, numeric, Brazilian document validators (CPF, CNPJ, phone, CEP), password complexity attributes, and data annotations
- **NuvTools.Validation.AspNetCore.Blazor** - Blazor integration for FluentValidation with EditContext and ValidationMessageStore support
- **NuvTools.Validation.Tests** - Test project using NUnit

All libraries are published as NuGet packages and target .NET 8, .NET 9, and .NET 10.

## Build and Test Commands

**Note:** This solution uses the modern `.slnx` (XML-based solution) format introduced in Visual Studio 2022 v17.11.

### Build the solution
```bash
dotnet build NuvTools.Validation.slnx
```

### Build for specific configuration
```bash
dotnet build NuvTools.Validation.slnx --configuration Release
```

### Run all tests
```bash
dotnet test NuvTools.Validation.slnx
```

### Run tests for specific project
```bash
dotnet test tests/NuvTools.Validation.Test/NuvTools.Validation.Tests.csproj
```

### Run a single test
```bash
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"
```

## Architecture and Key Components

### NuvTools.Validation Library

Core validation library organized into three functional areas.

#### General Validators (`NuvTools.Validation`)

**Validator** - Extension methods for common validations:
- `IsEmail()` - Email validation using source-generated regex
- `HasNumbersOnly()` - Checks digits-only strings
- `IsIntNumber()`, `IsLongNumber()` - Integer/long validation with optional `positiveOnly` flag
- `IsDecimalNumber()` - Decimal validation with culture-aware format provider

**RegexPattern** - Pre-compiled regex patterns via `[GeneratedRegex]`:
- `EmailRegex()` - Strict email validation (supports domain and IPv4)
- `Base64ContentRegex()` - Raw Base64 content validation
- `Base64DataUriRegex()` - Data URI with Base64 payload validation

**ValidatorHelper** - Internal helper (`GetNumbersOnly()` for stripping non-digit characters)

#### Brazilian Validators (`NuvTools.Validation.Brazil`)

**Validator** - Extension methods for Brazilian document validation:
- `IsCPF()` - CPF validation with checksum algorithm (formatted or unformatted)
- `IsCNPJ()` - CNPJ validation with checksum algorithm (formatted or unformatted)
- `IsCPForCNPJ()` - Auto-detects CPF (11 digits) or CNPJ (14 digits)
- `IsMobileNumber()` - Brazilian mobile number with valid area codes (DDD)
- `IsZipCodeNumber()` - CEP validation (XXXXX-XXX or XXXXXXXX)

**Format** - Extension methods for document formatting:
- `FormatCPF()` - Formats to XXX.XXX.XXX-XX
- `FormatCNPJ()` - Formats to XX.XXX.XXX/XXXX-XX

**RegexPattern** - Brazilian-specific compiled regex:
- `MobileNumberRegex()` - Mobile numbers with valid area codes (11-99)
- `ZipCodeRegex()` - CEP format validation

#### Data Annotations (`NuvTools.Validation.Annotations`)

**PasswordComplexityBaseAttribute** - Abstract base for password complexity:
- `PasswordComplexityCapitalLettersAttribute(int minOccurrences)` - Minimum uppercase letters
- `PasswordComplexityLowerCaseLettersAttribute(int minOccurrences)` - Minimum lowercase letters
- `PasswordComplexityDigitsAttribute(int minOccurrences)` - Minimum digits

**StringValueBaseAttribute** - Base class for string-based validation attributes

**Validator** (Annotations) - Extension method:
- `Validate<T>()` - Validates object using DataAnnotation attributes, returns error list or null

#### Brazilian Data Annotations (`NuvTools.Validation.Brazil.Annotations`)

- `CPFAttribute` - CPF validation attribute
- `CNPJAttribute` - CNPJ validation attribute

### NuvTools.Validation.AspNetCore.Blazor Library

Blazor integration for FluentValidation.

**FluentValidation\<TModel\>** - Bridges FluentValidation with Blazor's EditContext:
- Constructor parameters: model, validator, editContext, autoValidationOnRequested, autoValidationOnFieldChanged, highlightInvalidFields
- `Validate()` - Validates entire model and populates ValidationMessageStore
- `ValidateField()` - Validates a single field by FieldIdentifier
- `ToFieldIdentifier()` (private) - Resolves nested property paths (e.g., "Address.City") by traversing the object graph

Key design pattern: The `ToFieldIdentifier()` method handles property path resolution for nested objects, ensuring validation errors bind correctly to nested properties in Blazor forms.

## Localization

Validation messages are stored in `Resources/Messages.resx` using PublicResXFileCodeGenerator for strongly-typed access via `Messages.Designer.cs`.

## Code Style and Conventions

- **Nullable reference types** are enabled (`<Nullable>enable</Nullable>`)
- **Implicit usings** are enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- **Code style enforcement** is enabled during build (`<EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>`)
- **XML documentation generation** is required (`<GenerateDocumentationFile>True</GenerateDocumentationFile>`)
- Uses source-generated regex via `[GeneratedRegex]` attribute
- Uses modern C# collection expressions

## Testing

Tests use NUnit 4.x and target only `net10.0`. Patterns:
- `[TestFixture]` for test classes
- `[Test]` for test methods
- `Assert.That()` syntax with constraints
- Test helpers in `ValidatorExtensions.cs` and `Brazil/BrazilValidatorExtensions.cs`

## Dependencies

### NuvTools.Validation
- No external package dependencies

### NuvTools.Validation.AspNetCore.Blazor
- FluentValidation [12.1.1,13.0.0)
- Microsoft.AspNetCore.Components.Forms (version varies by target framework)

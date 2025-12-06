# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NuvTools Validation is a multi-targeted .NET validation library (supporting .NET 8, 9, and 10) designed for Web, Desktop, and Mobile (MAUI) applications. The solution consists of two main libraries:

1. **NuvTools.Validation** - Core validation library with Brazilian document validators, regex patterns, and data annotations
2. **NuvTools.Validation.AspNetCore.Blazor** - Blazor-specific extension integrating FluentValidation with EditContext

## Building and Testing

### Build the solution
```bash
dotnet build NuvTools.Validation.slnx
```

### Build specific configuration
```bash
dotnet build NuvTools.Validation.slnx -c Release
```

### Run all tests
```bash
dotnet test tests/NuvTools.Validation.Test/NuvTools.Validation.Tests.csproj
```

### Run a specific test
```bash
dotnet test tests/NuvTools.Validation.Test/NuvTools.Validation.Tests.csproj --filter "FullyQualifiedName~FluentValidationTests.Validate_ModelWithErrors_ReturnsValidationResult"
```

### Create NuGet packages
```bash
dotnet pack NuvTools.Validation.slnx -c Release
```

## Architecture

### Core Validation Library (NuvTools.Validation)

The library is organized into three main functional areas:

**1. General Validators (`Validator.cs`)**
- Static extension methods for common validations (email, numeric types, decimals)
- Uses `RegexPattern.cs` for email and other pattern matching
- Helper methods in `ValidatorHelper.cs` (internal, e.g., `GetNumbersOnly()`)

**2. Brazilian Validators (`Brazil/Validator.cs`)**
- Specialized validators for Brazilian documents: CPF, CNPJ
- Brazilian mobile number and zip code validation
- Uses `Brazil/RegexPattern.cs` for mobile and zip code patterns
- Formatting utilities in `Brazil/Format.cs` for CPF/CNPJ display

**3. Data Annotations (`Annotations/`)**
- Custom validation attributes extending `ValidationAttribute`
- Password complexity validators (base class, capital letters, digits, lowercase)
- Brazilian document attributes (`Brazil/Annotations/CPFAttribute.cs`, `CNPJAttribute.cs`)
- `StringValueBaseAttribute` for string-based validations

### Blazor Integration Library (NuvTools.Validation.AspNetCore.Blazor)

**FluentValidation Integration (`FluentValidation.cs`)**
- Bridges FluentValidation with Blazor's EditContext and ValidationMessageStore
- Supports both full model validation and single field validation
- Handles nested property paths (e.g., "Address.City") by traversing object graphs
- Auto-validation hooks for `OnValidationRequested` and `OnFieldChanged` events
- `FluentValidation/PropertyValidatorBase.cs` provides base functionality for custom validators

**Key Design Pattern:**
The `ToFieldIdentifier()` method handles property path resolution for nested objects, ensuring validation errors bind correctly to nested properties in Blazor forms.

## Multi-Targeting Strategy

All libraries target .NET 8, 9, and 10 simultaneously via `<TargetFrameworks>net8;net9;net10.0</TargetFrameworks>`. The Blazor library conditionally references the appropriate `Microsoft.AspNetCore.Components.Forms` version based on the target framework.

## Localization

Validation messages are stored in `Resources/Messages.resx` using PublicResXFileCodeGenerator for strongly-typed access via `Messages.Designer.cs`.

## Code Style

- `ImplicitUsings` and `Nullable` enabled project-wide
- `EnforceCodeStyleInBuild` is enabled
- Uses C# latest language version with modern patterns (e.g., collection expressions `[10, 9, 8, ...]`)
- Partial methods for source-generated regex (`[GeneratedRegex]` attribute)

## Testing Framework

Tests use NUnit 4.x with the following patterns:
- `[TestFixture]` for test classes
- `[Test]` for test methods
- Assert.That() syntax with constraints
- Test helpers in `ValidatorExtensions.cs` and `Brazil/BrazilValidatorExtensions.cs`

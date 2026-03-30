# ProjectBuilders

[![License](https://img.shields.io/github/license/dailydevops/projectbuilders.svg)](LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/dailydevops/projectbuilders/ci.yml?branch=main)](https://github.com/dailydevops/projectbuilders/actions)
ProjectBuilders is a .NET solution providing libraries and utilities for programmatically building, managing, and testing .NET project structures. It is designed for developers, tool authors, and CI/CD engineers who need to automate .NET project scaffolding, configuration, and test environment setup. The solution aims to simplify the creation and management of .NET projects and their test environments, supporting modern .NET development workflows.

## Overview

This solution contains multiple projects organized into a cohesive architecture:

- **Core Libraries**: Foundation for project building and shared abstractions
- **Test Utilities**: Helpers and fixtures for integration with TUnit and xUnit
- **Test Projects**: Comprehensive unit and integration tests for all components

## Projects

### Core Libraries

- [**NetEvolve.ProjectBuilders**](src/NetEvolve.ProjectBuilders/README.md) - Core library for programmatic .NET project building and management

### Test Utilities

- [**NetEvolve.ProjectBuilders.TUnit**](src/NetEvolve.ProjectBuilders.TUnit/README.md) - Test utilities for TUnit-based projects
- [**NetEvolve.ProjectBuilders.XUnit**](src/NetEvolve.ProjectBuilders.XUnit/README.md) - Test utilities for xUnit-based projects

### Tests

- **NetEvolve.ProjectBuilders.Tests.Unit** - Unit tests for core components
- **NetEvolve.ProjectBuilders.Tests.Integration** - Integration tests for core components
- **NetEvolve.ProjectBuilders.TUnit.Tests.Integration** - Integration tests for TUnit utilities
- **NetEvolve.ProjectBuilders.XUnit.Tests.Integration** - Integration tests for xUnit utilities

## Features

- Programmatic creation and manipulation of .NET project files
- Utilities for managing solution and project references
- Helpers for test environment setup and teardown
- Extensible abstractions for custom project builders

## Getting Started

### Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or higher
- [Git](https://git-scm.com/) for version control
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (recommended)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/dailydevops/projectbuilders.git
   cd projectbuilders
   ```

### Configuration

No additional configuration is required for basic usage.

## Development

### Building

```bash
dotnet build
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/NetEvolve.ProjectBuilders.Tests.Unit
```

### Code Formatting

```bash
# Format code using CSharpier
csharpier format .
```

### Project Structure

```txt
src/                    # Production code
├── NetEvolve.ProjectBuilders/         # Core library
├── NetEvolve.ProjectBuilders.TUnit/   # TUnit test utilities
└── NetEvolve.ProjectBuilders.XUnit/   # xUnit test utilities

tests/                 # Test projects
├── NetEvolve.ProjectBuilders.Tests.Unit/         # Unit tests
├── NetEvolve.ProjectBuilders.Tests.Integration/  # Integration tests
├── NetEvolve.ProjectBuilders.TUnit.Tests.Integration/ # TUnit integration tests
└── NetEvolve.ProjectBuilders.XUnit.Tests.Integration/ # xUnit integration tests

decisions/            # Architecture Decision Records (ADRs)
templates/            # Documentation and file templates
```

## Architecture

This solution follows modern .NET architectural patterns and best practices:

- **Clean Architecture**: Clear separation of concerns with dependencies pointing inward
- **Dependency Injection**: Built-in .NET dependency injection throughout
- **Extensible Abstractions**: Interfaces and extension points for custom builders
- **SOLID Principles**: Object-oriented design with SOLID principles

For detailed architectural decisions, see the [Architecture Decision Records](decisions/).

## Contributing

We welcome contributions from the community! Please read our [Contributing Guidelines](CONTRIBUTING.md) before submitting a pull request.

Key points:

- Follow the [Conventional Commits](https://www.conventionalcommits.org/) format for commit messages
- Write tests for new functionality
- Follow existing code style and conventions
- Update documentation as needed

## Code of Conduct

This project adheres to the Contributor Covenant [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to [info@daily-devops.net](mailto:info@daily-devops.net).

## Documentation

- **[Architecture Decision Records](decisions/)** - Detailed architectural decisions and rationale
- **[Contributing Guidelines](CONTRIBUTING.md)** - How to contribute to this project
- **[Code of Conduct](CODE_OF_CONDUCT.md)** - Community standards and expectations
- **[License](LICENSE)** - Project licensing information

## Versioning

This project uses [GitVersion](https://gitversion.net/) for automated semantic versioning based on Git history and [Conventional Commits](https://www.conventionalcommits.org/). Version numbers are automatically calculated during the build process.

## Support

- **Issues**: Report bugs or request features on [GitHub Issues](https://github.com/dailydevops/projectbuilders/issues)
- **Documentation**: Read the full documentation in this repository

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

> [!NOTE]
> **Made with ❤️ by the NetEvolve Team**
> Visit us at [https://www.daily-devops.net](https://www.daily-devops.net) for more information about our services and solutions.

# Smab.TTInfo.CunninghamCup

## Overview
Smab.TTInfo.CunninghamCup is a cross-platform table tennis tournament information app built with .NET MAUI and Blazor. It provides tools for managing, viewing, and analyzing tournament draws, rounds, and results. The solution is designed to run on Android, iOS, Windows, MacCatalyst, and the web.

## Features
- Cross-platform support (Android, iOS, Windows, MacCatalyst, Web)
- Tournament draw and round management
- Extensions for tournament logic and knockout rounds
- Modern UI with .NET MAUI and Blazor
- Unit tests for core logic

## Technologies Used
- [.NET 10](https://dotnet.microsoft.com/)
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/)
- [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- C# 13
- xUnit (for testing)

## Getting Started
### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 or later (with MAUI and Blazor workloads)

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/smabuk/Smab.TTInfo.CunninghamCup.git
   ```
2. Open the solution in Visual Studio.

### Build and Run
- **MAUI App:**
  - Select the `Smab.TTInfo.CunninghamCup` project and choose your target platform (Android, iOS, Windows, MacCatalyst).
  - Click Run.
- **Web App:**
  - Set `Smab.TTInfo.CunninghamCup.Web` as the startup project.
  - Run the project to launch the Blazor web app.

## Project Structure
```
src/
  Smab.TTInfo.CunninghamCup/           # .NET MAUI app (multi-platform)
  Smab.TTInfo.CunninghamCup.Shared/     # Shared logic, extensions, and pages
  Smab.TTInfo.CunninghamCup.Web/        # Blazor web app
  ...
tests/
  Smab.TTInfo.CunninghamCup.Tests/      # Unit tests
```

## Testing
Run unit tests with:
```sh
dotnet test tests/Smab.TTInfo.CunninghamCup.Tests
```

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request. For major changes, open an issue first to discuss what you would like to change.

## License
This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.

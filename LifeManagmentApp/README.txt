# LifeManagementApp

A mobile life management application developed using **.NET MAUI, C#, and XAML**.

The application helps users organize their daily tasks and check current weather conditions, including whether the wind is suitable for kitesurfing.

This project was developed as a mobile application coursework assignment.

## Features

### Home Page

- Displays the three highest-priority unfinished tasks.
- Tasks are ordered by priority (highest first), then by due date (earliest first).
- Shows current temperature in degrees Celsius.
- Shows current wind speed in meters per second.
- Displays a kitesurfing alert when wind speed exceeds 5 m/s.
- Retrieves weather information from the Open-Meteo REST API.
- Supports refreshing the displayed information.

### To-do List

- Add new tasks.
- Edit existing tasks.
- Delete tasks.
- Mark tasks as completed or unfinished.
- Assign a priority from 1 to 5.
- Select a due date.
- Sort tasks by priority or due date.
- Filter tasks by All, Active, or Completed.
- Validate task titles to prevent empty entries.
- Display task status as Pending or Completed using a XAML value converter.

### Data Storage

Tasks are stored locally using **SQLite and Entity Framework Core**.

The application uses asynchronous database operations to keep the user interface responsive.

Saved tasks remain available after closing and reopening the application.

## Technologies

| Technology | Purpose |
|---|---|
| .NET 9 MAUI | Cross-platform mobile application framework |
| C# | Application logic |
| XAML | User interface |
| CommunityToolkit.Mvvm | MVVM properties and commands |
| Entity Framework Core | Database access |
| SQLite | Local persistent storage |
| HttpClient | REST API requests |
| Open-Meteo API | Weather information |
| Dependency Injection | Service and ViewModel management |
| Git and GitHub | Version control |

## Application Architecture

The application follows the **Model-View-ViewModel (MVVM)** architecture.

### Models

Models represent the application's data.

- `TodoItem.cs` — task ID, title, priority, due date, and completion status.
- `WeatherInfo.cs` — temperature, wind speed, and kitesurfing conditions.

### Views

Views are written in XAML and display information to the user.

- `HomePage.xaml` — home screen with tasks and weather.
- `TodoListPage.xaml` — task management screen.

### ViewModels

ViewModels manage UI state, commands, and data binding.

- `HomeViewModel.cs` — loads the top three unfinished tasks and current weather.
- `TodoListViewModel.cs` — manages task creation, editing, deletion, completion, filtering, and sorting.

CommunityToolkit.Mvvm provides `ObservableObject`, `[ObservableProperty]`, and `[RelayCommand]` to reduce repetitive code.

### Services

Services handle application operations outside the user interface.

- `ITodoRepository.cs` — defines task storage operations.
- `TodoService.cs` — implements task operations using Entity Framework Core.
- `DatabaseInitializationService.cs` — initializes the SQLite database.
- `IWeatherService.cs` — defines the weather service interface.
- `OpenMeteoWeatherService.cs` — retrieves weather information from Open-Meteo.

### Data

- `AppDbContext.cs` — Entity Framework Core database context.

### Converters

- `BoolToTextConverter.cs` — converts the task completion Boolean value into Pending or Completed text.

## Dependency Injection

Dependency injection is configured in `MauiProgram.cs`.

The application registers its database context factory, services, ViewModels, and pages.

Interfaces such as `ITodoRepository` and `IWeatherService` allow ViewModels to depend on abstractions rather than specific implementations.

This improves separation of responsibilities, maintainability, and testability.

## Database

The application uses SQLite for local data storage.

Each task contains the following properties:

| Property | Description |
|---|---|
| Id | Unique task identifier |
| Title | Task description |
| Priority | Priority value from 1 to 5 |
| DueDate | Task deadline |
| IsDone | Completion status |

Entity Framework Core provides asynchronous methods for reading, adding, updating, and deleting tasks.

The database is stored in the application's local data directory.

`DatabaseInitializationService` uses `EnsureCreatedAsync()` to create the database when necessary.

## Weather API

Weather information is retrieved from the free **Open-Meteo API**.

API website: https://open-meteo.com/

The application requests current weather conditions for Vaasa, Finland.

The response includes:

- Current temperature in °C.
- Current wind speed in m/s.

The application displays a kitesurfing alert when:

`WindSpeed > 5`

The weather service uses `HttpClient` and JSON deserialization.

If weather retrieval fails, the application displays an error message instead of crashing.

## Installation and Running

### Requirements

- Visual Studio with the .NET MAUI workload installed.
- .NET 9 SDK.
- Android emulator or compatible Android device.

### Steps

1. Clone the GitHub repository.
2. Open `LifeManagementApp.slnx` in Visual Studio.
3. Restore NuGet packages if necessary.
4. Select the Android target.
5. Start an Android emulator or connect a device.
6. Build and run the application.

An internet connection is required to retrieve weather information.

Task management and local SQLite storage do not require internet access.

## Git and Version Control

Git and GitHub were used to track development progress.

Development included:

- Creating commits during implementation.
- Working on a separate development branch.
- Merging the working branch into `master`.
- Synchronizing local changes with GitHub.

## AI Assistance

AI assistance, including ChatGPT, was used during the development of this project.

AI was used to help with:

- Understanding .NET MAUI and MVVM concepts.
- Generating and reviewing C# and XAML code.
- Troubleshooting build and runtime errors.
- Implementing SQLite database operations with Entity Framework Core.
- Configuring dependency injection and interfaces.
- Integrating the Open-Meteo REST API.
- Improving code organization.
- Preparing project documentation.

The application was developed, built, tested, and debugged in Visual Studio. AI-generated suggestions were reviewed and adapted during implementation.

## Future Improvements

Possible future enhancements include:

- Task categories.
- Task reminders and notifications.
- Configurable weather locations.
- Additional automated tests.
- More advanced task search and filtering.

These features are not currently implemented.

## Author

Developed as a .NET MAUI mobile application coursework project.
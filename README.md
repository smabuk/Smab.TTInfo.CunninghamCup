# Cunningham Cup Tournament Site

A Blazor-powered web app for running the OLOP “Cunningham Cup” handicap table-tennis
tournament. It supports drawing round-robin groups, entering results, calculating
qualifiers, and running the main and consolation knockout stages.
State is persisted to JSON files for resilience and auditing.

## Solution overview

Projects
- ```src/Smab.TTInfo.CunninghamCup.Shared``` (Razor class library)
  - Components, pages, domain models, and services shared with the host
  - Core models: ```Tournament```, ```Group```, ```Player```, ```Match```, ```KnockoutStage```, ```KnockoutRound```, ```PlayerId```, ```MatchId```
  - Services: ```ITournamentService``` (JSON persistence + audit logging), ```TTClubsReader``` (external data integration, cached)
- ```src/Smab.TTInfo.CunninghamCup.Web``` (Blazor SSR host)
  - Hosts the UI using Blazor Server render mode for interactivity
- ```tests/Smab.TTInfo.CunninghamCup.Tests``` (unit tests)

Target framework and language
- .NET 10 (preview)
- C# 13 features (records, collection expressions, pattern matching, new extension member syntax, etc.)

Key UI pages
- / (Home) – tournament intro, title, and date
- /edit/create – admin: create/update tournament and player list, dev helpers for random seeding
- [/edit]/entries – QuickGrid of players, handicaps, and groups
- [/edit]/group – list of groups, schedule times, and summary; admin draw groups from here
- [/edit]/group/\{GroupName\} – round-robin group details; enter/edit results per match
- [/edit]/knockout – draw and manage main and consolation knockout brackets
- /summary – qualifiers, bracket summaries, and winners once available
- /information – event schedule and rules

Admin/editing
- Admin routes use ```<CanEdit/>```/```<CantEdit/>``` components to gate editing UI
- Extra developer-only tools (e.g., random results, reset) are shown when ```ASPNETCORE_ENVIRONMENT=Development```

Persistence and logging
- ```ITournamentService``` saves the live state to cache folder as JSON: tournament_\{yyyy\}.json plus timestamped snapshots
- Audit log appended to tournament_\{yyyy\}.log

## Domain model and flow

```Tournament```
- ```Name```, ```Date```, ```Players``` (```Dictionary<PlayerId, Player>```), ```Groups``` (```List<Group>```), ```KnockoutStage```, ```ConsolationStage```
- DrawGroups(int groupSize) distributes active players across lettered groups (A..), assigning two start times by default

```Group```
- ```Players``` (```List<PlayerId>```) participate in a round-robin; Matches capture per-set scores
- ```GroupPositions``` compute ranking for qualifiers; ```IsCompleted``` indicates all matches finished

```Match```
- ```PlayerA```/```PlayerB``` (```PlayerId```) may be a real player, a ```Bye```, or a placeholder during knockout seeding
- Best of 3 to 21; ```Result``` holds set scores; ```IsCompleted``` and Winner/Loser derived
- Handicap start points computed from ```Tournament.StartingHandicap(playerA, playerB)```

```KnockoutStage``` and ```KnockoutRound```
- ```DrawKnockoutStage(name, [positions])``` builds bracket with placeholders
-  (e.g., Group A Winner vs Group D Runner Up) and automatically carries forward
-  winners to later rounds once results are entered
- ```UpdateMainKnockoutRounds```/```UpdateConsolationRounds``` populate brackets as groups finish

Service layer (ITournamentService)
- ```GetTournament()```/```AddOrUpdateTournament()```
- ```LoadTournamentFromJsonAsync()```/```SaveTournamentToJsonAsync(stage?)``` for persistence and snapshotting
- ```LogToAuditFile(message)``` for operational traceability
- Extension helpers: ```DrawGroups```, ```DrawMainKnockoutStage```, ```DrawConsolationStage```, ```UpdateRounds```

External data and caching
- TTClubsReader integrates with ttleagues API when needed and caches JSON responses
- File-based cache via CacheHelper; configurable via options

## Prerequisites

Because this project targets .NET 10 preview and Blazor 10 preview packages, use a matching SDK/IDE:
- .NET SDK 10.0.100-preview (preview 7 or later matching the package versions)
- Visual Studio 2022 17.12 Preview or VS Code + C# Dev Kit
- Optional: Git

Check SDK
- dotnet --list-sdks

Install from https://dotnet.microsoft.com/ if needed.

## Clone and restore

- git clone https://github.com/smabuk/Smab.TTInfo.CunninghamCup.git
- cd Smab.TTInfo.CunninghamCup
- dotnet restore

## Local configuration

The app uses Options binding (section TTInfo) and a file-based cache folder for state.

Add appsettings.Development.json in ```src/Smab.TTInfo.CunninghamCup.Web``` if you want to override defaults:

```codejson
{
  "TTInfo": {
    "CacheFolder": "cache",
    "CacheHours": 6,
    "UseTestFiles": false,
    "MagicWord": "please"
  }
}
```

Notes
- CacheFolder must be writable by the app. Default relative folder “cache” will be created if missing
- User secrets are enabled on the Web project; you can also store the TTInfo section using dotnet user-secrets

## Run locally

Option 1 – Visual Studio
- Set ```Smab.TTInfo.CunninghamCup.Web``` as startup project
- Run (Ctrl+F5/F5). The app uses Blazor SSR with InteractiveServer render mode

Option 2 – CLI
- dotnet build
- dotnet run --project src/Smab.TTInfo.CunninghamCup.Web
- Open http://localhost:5000 (or the URL shown in console)

## Operating a tournament

1) Create or update the tournament
- Navigate to /edit/create (requires admin). Enter name and date
- Add players (Name, optional TTE Id, Handicap, Withdrawn flag)
- Use Update tournament to save. In Development you can seed a random tournament

2) Draw group stage
- Go to /group. If no groups exist, choose Group size and click Draw Groups
- Start times are assigned (e.g., 09:30 for earlier groups, 11:00 for later)

3) Enter group results
- Open /group/\{GroupName\}
- Click the pencil to edit a match. Enter set scores (best of 3 to 21)
- Completed groups automatically determine positions (Winner, Runner Up, Third, Fourth)

4) Draw knockouts
- Go to /knockout
- Draw Main Knockout (top 2 from each group) and Draw Consolation (3rd/4th)
- As results are entered, click Update Main Knockout/Update Consolation to propagate winners and compute handicap starts

5) Track outcomes
- /summary shows qualifiers, bracket progress, and final podium once semi/finals complete
- /entries lists all entrants with handicaps and group assignment
- /information shows the day schedule and rules

6) Persistence and backup
- The service automatically writes timestamped snapshots and a current season file to the cache folder
- Keep the cache folder backed up if running live

## Deployment

- Build Release and deploy the Web project to your target (Windows service, IIS, Linux systemd, container)
- Ensure the .NET 10 preview runtime matching your build is available on the server
- Configure ```TTInfo:CacheFolder``` to a persistent writable path
- Expose HTTPS via your preferred reverse proxy (IIS, Nginx, Apache, Azure App Service)
- Preserve the cache directory across updates

## Testing

- dotnet test

## Technologies and concepts

- Blazor SSR with Interactive Server render mode
- QuickGrid for tabular data
- Dependency Injection (```services.AddSingleton<ITournamentService>```)
- Options pattern with validation (```TTInfoOptions```)
- Records, collection expressions, and modern C# patterns
- File-based persistence and audit logging
- External API access with ```HttpClient``` and caching (```TTClubsReader```)
- DataAnnotations validation in forms
- Component state via ```[PersistentState]```
- Environment-specific behavior (Development helpers)

## Troubleshooting

- No editing controls visible: ensure you’re on an /edit route and admin/edit mode is enabled in your hosting setup
- State not saving: verify ```TTInfo:CacheFolder``` is writable
- Build errors: confirm you’re on a .NET 10 preview SDK matching the package versions in the csproj files

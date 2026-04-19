# Watchlist & Alert System

A full-stack currency watchlist and alert application. Track currency pairs, set price alerts, and view historical exchange rates — powered by the Frankfurter API.

## Tech Stack
 
| Layer | Technology |
|-------|-----------|
| Backend | .NET 8 Web API |
| ORM | Entity Framework Core |
| Database | SQLite |
| Frontend | React + Vite |
| E2E Testing | Cypress |
| Unit Testing | xUnit + Moq |
| External API | [Frankfurter](https://www.frankfurter.app/) |


## How to Run the Backend
 
### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
### Steps
 
```bash
# 1. Clone the repository
git clone <repo-url>
cd backend
 
# 2. Apply database migrations
# Default database: currency_watchlist.db (SQLite — no config needed)
dotnet ef database update
 
# 3. Run the API
dotnet run
# or for hot reload
dotnet watch run
```
 
The API starts on `http://localhost:5109`.

---

## How to Run the Frontend
 
### Prerequisites
- [Node.js 18+](https://nodejs.org/)
### Steps
 
```bash
# 1. Navigate to the frontend directory
cd frontend
 
# 2. Install dependencies
npm install
 
# 3. Create environment file
echo "VITE_API_BASE_URL=/api" > .env.development
 
# 4. Start the dev server
npm run dev
```

The app starts on `http://localhost:3000`.
 
> All `/api` requests are proxied through Vite to `http://localhost:5109` — the backend URL is never exposed to the browser.


## How to Run Unit Tests
 
```bash
# From root — create the test project (first time only)
dotnet new xunit -n backend.Tests
dotnet add backend.Tests reference backend
dotnet restore
 
# Install test packages
cd backend.Tests
dotnet add package xunit
dotnet add package Moq
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.runner.visualstudio
 
# Run tests
dotnet test
```

## External API

- This application uses the Frankfurter API to fetch exchange rates: 
- https://www.frankfurter.app/


## Project Structrue

backend/
│
├── Controllers/
│   ├── RatesController.cs
│   ├── WatchlistsController.cs
│   ├── WatchlistsItemController.cs
│   └── AlertRuleController.cs
│
├── Services/
│   ├── RateRefreshService
│   ├── LatestRateService
│   ├── HistoryRateService
│
├── Repositories/
│   ├── WatchlistRepository
│   ├── WatchlistItemRepository
│
├── Models/
|	├── AlertEvent
│   ├── AlertRule
│   ├── RateSnapShot
│   ├── Watchlist
│   ├── WatchlistItem
│
├── Dtos/
│   ├── Alert/AlertRuleListDto
│   ├── Alert/CreateAlertRuleRequestDto
│   ├── External/ExternalApiRawResponse
│   ├── Rate/RateSnapShotDto
│   ├── Watchlist/CreateWatchlistRequestDto
│   ├── Watchlist/WatchlistDto
│   ├── WatchlistItem/CreateWatchlistItemRequestDto
│   ├── WatchlistItem/WatchlistItemDto
│   ├── WatchlistItem/watchlistItemSummaryDto
|
├── Mappers/
│   ├── AlertRuleMapper
│   ├── WatchlistItemMappers
│   ├── WatchlistMappers
|
├── Helpers/
│   └── ApiResponse.cs
│
└── Data/
    └── ApplicationDbContext.cs


## Base URL

/api

## Controllers

### 1. AlertRule - /api/AlertRule

| Method | Endpoint                       | Description                   |
| ------ | ------------------------------ | ----------------------------- |
| GET    | `/api/AlertRule`               | Get all alert rules           |
| GET    | `/api/AlertRule/{id}`          | Get a single alert rule by ID |
| POST   | `/api/AlertRule`               | Create a new alert rule       |
| DELETE | `/api/AlertRule/{id}`          | Delete an alert rule by ID    |
| POST   | `/api/AlertRule/{id}/evaluate` | Evaluate Alert                |


### 2. Rates — `/api/Rates`

| Method | Endpoint             | Description                                                |
| ------ | -------------------- | ---------------------------------------------------------- |
| POST   | `/api/Rates/refresh` | Triggers a rate refresh for all watchlist pairs            |
| GET    | `/api/Rates/latest`  | Get the latest rate for a currency pair                    |
| GET    | `/api/Rates/history` | Get historical rates for a currency pair over a date range |

### 3. Watchlists — `/api/Watchlists`

| Method | Endpoint               | Description                         |
| ------ | ---------------------- | ----------------------------------- |
| GET    | `/api/Watchlists`      | Get all watchlists (items included) |
| GET    | `/api/Watchlists/{id}` | Get a single watchlist by ID        |
| POST   | `/api/Watchlists`      | Create a new watchlist              |
| DELETE | `/api/Watchlists/{id}` | Delete a watchlist by ID            |

### 4. Watchlist Items — `/api/watchlists/{watchlistId}/items`

| Method | Endpoint                                   | Description                             |
| ------ | ------------------------------------------ | --------------------------------------- |
| GET    | `/api/watchlists/{watchlistId}/items/{id}` | Get a single item to its watchlist      |
| POST   | `/api/watchlists/{watchlistId}/items`      | Add a currency pair item to a watchlist |
| DELETE | `/api/watchlists/{watchlistId}/items/{id}` | Delete an item by ID                    |


## Assumption / Tradeoffs Made:

 - Single snapshot per currency pair.
 - No authentication.
 - Hard deletes only. 
 - Duplicate watchlist items are blocked.
 - Alert Evaluation on demand.


---

## Future Improvements

- Add Authentication.
- Background Refresh Rate.
- Currency Code Validation.
- Soft Delete.
- Pagination.






# Backend API Documentation

## Installation

- Follow these steps to set up and run the backend API locally:

- Clone the Repository
- Configure Database - Default set to currency_watchlist.db
- Run Database Migration - command dotnet ef database update
- Run the Application - dotnet run / dotnet watch run

## Unit Testing Installation

# From root
- dotnet new xunit -n backend.Tests
- dotnet add backend.Tests reference backend
- dotnet restore
# Move into test project
- cd backend.Tests
# Install packages
- dotnet add package xunit
- dotnet add package Moq
- dotnet add package Microsoft.NET.Test.Sdk
- dotnet add package xunit.runner.visualstudio
# Run tests
- dotnet test


## Overview

This API provides exchange rate tracking and watchlist management. It integrates with the Frankfurter external rate API and stores data using Entity Framework Core.

## Tech Stack
- .NET 8 Web API
- Entity Framework Core
- SQLite
- xUnit (for testing)
- Moq (for unit testing)

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
| POST   | `/api/AlertRule/{id}/evaluate` | Evaluate Alert Rule           |


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







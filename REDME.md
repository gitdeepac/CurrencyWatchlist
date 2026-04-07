# Backend API Documentation

## Installation

- Follow these steps to set up and run the backend API locally:

- Clone the Repository
- Configure Database - Default set to currency_watchlist.db
- Run Database Migration - command dotnet ef database update
- Run the Application - dotnet run / dotnet watch run

## Overview

This API provides exchange rate tracking and watchlist management. It integrates with the Frankfurter external rate API and stores data using Entity Framework Core.

## Base URL

/api

## Controllers

### 1. AlertRule - /api/AlertRule

| Method | Endpoint              | Description                   |
| ------ | --------------------- | ----------------------------- |
| GET    | `/api/AlertRule`      | Get all alert rules           |
| GET    | `/api/AlertRule/{id}` | Get a single alert rule by ID |
| POST   | `/api/AlertRule`      | Create a new alert rule       |
| DELETE | `/api/AlertRule/{id}` | Delete an alert rule by ID    |

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

# FrontEnd

### Install Required Tool

- Got to frontend folder
- Run : npm install
- Install concurrently (used to run multiple commands at once):
  - Command : npm install concurrently --save-dev

// src/router/index.tsx
import { createBrowserRouter } from "react-router-dom";
import Navbar from "../components/layout/Navbar";
import ListWatchlist from "../pages/watchlist/ListWatchlist";
import CreateWatchlist from "../pages/watchlist/CreateWatchlist";
import ListWatchlistItem from "../pages/watchlistItem/ListWatchlistItem";
import CreateWatchlistItems from "../pages/watchlistItem/CreateWatchlistItems";
import RateListService from "../pages/rates/ListRates";
import ListAlerts from "../pages/alert/ListAlerts";
import CreateListAlert from "../pages/alert/CreateListAlert";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Navbar />,
    children: [
      { index: true, element: <ListWatchlist /> },
      { path: "watchlist", element: <ListWatchlist /> },
      { path: "watchlist/add", element: <CreateWatchlist /> },
	  { path: "watchlistItems/:watchlistId", element: <ListWatchlistItem /> },
	  { path: "watchlistItems/:watchlistId/add", element: <CreateWatchlistItems /> },
	  { path: "rateService", element: <RateListService /> },
	  { path: "watchlistItems/:watchlistId/alertService/:watchlistItemId", element: <ListAlerts /> },
	  { path: "watchlistItems/:watchlistId/alertService/:watchlistId/add", element: <CreateListAlert /> },
    ],
  },
  //   { path: "*", element: <NotFound /> },
]);

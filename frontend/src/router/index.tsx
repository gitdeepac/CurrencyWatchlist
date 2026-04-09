// src/router/index.tsx
import { createBrowserRouter } from "react-router-dom";
import Navbar from "../components/layout/Navbar";
import ListWatchlist from "../components/watchlist/ListWatchlist";
import CreateWatchlist from "../components/watchlist/CreateWatchlist";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Navbar />,
    children: [
      { index: true, element: <ListWatchlist /> },
      { path: "watchlist", element: <ListWatchlist /> },
      { path: "watchlist/add", element: <CreateWatchlist /> },
      //   { path: "watchlist-items", element: <WatchlistItems /> },
      //   { path: "alert-rules", element: <AlertRules /> },
    ],
  },
  //   { path: "*", element: <NotFound /> },
]);

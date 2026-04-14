import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import { watchlistApi } from "../../services/watchlist/api";
import { toast } from "react-toastify";

interface WatchlistItem {
  id: number;
  name: string;
}

const ListWatchlist = () => {
  const [watchlist, setWatchList] = useState<WatchlistItem[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isDeleting, setIsDeleting] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);

  const getWatchlist = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const watchListData = await watchlistApi.getAll();
      if (watchListData.success) {
        setWatchList(watchListData.data || []); // Fallback to empty array if response is null
      } else {
        setError(watchListData.message);
      }
    } catch (err) {
      setError("Failed to load watchlist. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    getWatchlist();
  }, []);

  const handleDelete = async (watchlistId: number) => {
    setIsDeleting(watchlistId);
    setError(null);

    // API call
    try {
      const deleteWatchListResp = await watchlistApi.delete(watchlistId);

      if (!deleteWatchListResp.success) {
        toast.error(deleteWatchListResp.message);
        return;
      }
      setWatchList((list) => list.filter((w) => w.id !== watchlistId));
      toast.success(deleteWatchListResp.data.message);
    } catch (err: any) {
      setError(err.response?.data?.message);
      toast.error(err.response?.data?.message);
    } finally {
      setIsDeleting(null);
    }
  };
  return (
    <div className="container-fluid">
      <div className="row mt-4">
        <div className="col-12 px-4">
          <div className="card">
            <div className="card-header d-flex justify-content-between">
              <h4 className="m-0">Watchlist - Listing</h4>
              <NavLink to="/watchlist/add" className="btn btn-primary">
                Create Watchlist
              </NavLink>
            </div>
            <div className="card-body">
              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}

              {isLoading ? (
                <p className="text-muted">Loading...</p>
              ) : (
                <table className="table table-striped">
                  <thead>
                    <tr>
                      <th scope="col">#</th>
                      <th scope="col">Watchlist Name</th>
                      <th scope="col">Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {watchlist.length === 0 ? (
                      <tr>
                        <td colSpan={3} className="text-center text-muted">
                          No watchlists found.
                        </td>
                      </tr>
                    ) : (
                      watchlist.map((item, index) => (
                        <tr key={item.id}>
                          <th scope="row">{index + 1}</th>
                          <td>{item.name}</td>
                          <td>
                            <button
                              className="btn btn-danger"
                              onClick={() => handleDelete(item.id)}
                              disabled={isDeleting === item.id}
                            >
                              {isDeleting === item.id
                                ? "Deleting..."
                                : "Delete"}
                            </button>
                          </td>
                        </tr>
                      ))
                    )}
                  </tbody>
                </table>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ListWatchlist;

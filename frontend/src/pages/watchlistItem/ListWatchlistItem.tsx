import { useEffect, useState } from "react";
import { NavLink, useLocation } from "react-router-dom";
import { toast } from "react-toastify";
import { watchlistItemApi } from "../../services/watchlistItem/api";

interface WatchlistItem {
  id: number;
  watchlistId: number;
  baseCurrency: string;
  quoteCurrency: string;
  createAt: string;
}
const ListWatchlistItem = () => {
  const location = useLocation();
  const [watchlistItemList, setWatchlistItemList] = useState<WatchlistItem[]>(
    [],
  );
  const [isLoading, setIsLoading] = useState(false);
  const [selectedId, setSelectedId] = useState(location.state?.id);
  const [isDeleting, setIsDeleting] = useState<number | null>(null);
  const [error, setError] = useState(null);

  const handleWatchlistListing = async (id) => {
    try {
      setIsLoading(true);
      setError(null);
      const watchListItemData = await watchlistItemApi.getByWatchlistIdAll(id);
      if (watchListItemData.success) {
        setWatchlistItemList(watchListItemData.data.data || []); // Fallback to empty array if response is null
      } else {
        setError(watchListItemData.message);
      }
    } catch (err) {
      setError("Failed to load watchlist Items. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleDelete = async (watchlistItemId: number) => {
    setIsDeleting(watchlistItemId);
    setError(null);

    // API call
    try {
      const deleteWatchListResp = await watchlistItemApi.delete(
        selectedId,
        watchlistItemId,
      );

      if (!deleteWatchListResp.success) {
        toast.error(deleteWatchListResp.message);
        return;
      }
      setWatchlistItemList((list) =>
        list.filter((w) => w.id !== watchlistItemId),
      );
      toast.success(deleteWatchListResp.data.message);
    } catch (err: any) {
      setError(err.response?.data?.message);
      toast.error(err.response?.data?.message);
    } finally {
      setIsDeleting(null);
    }
  };

  useEffect(() => {
    handleWatchlistListing(selectedId);
  }, [selectedId]);

  return (
    <div className="container-fluid">
      <div className="row mt-4">
        <div className="col-12 px-4">
          <div className="card">
            <div className="card-header d-flex justify-content-between align-items-center">
              <h4 className="m-0">Watchlist Item - Listing</h4>
              <div className="d-flex gap-2">
                <NavLink
                  to={`/watchlistItems/${selectedId}/add`}
                  className="btn btn-primary"
                  aria-disabled={watchlistItemList.length === 0 ? true : false}
                >
                  Create WatchlistItems
                </NavLink>
                <NavLink to={`/watchlist`} className="btn btn-success">
                  Back
                </NavLink>
              </div>
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
                      <th scope="col">Watchlist Id</th>
                      <th scope="col">Base Curreny</th>
                      <th scope="col">Quote Curreny </th>
                      <th scope="col">Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {watchlistItemList.length === 0 ? (
                      <tr>
                        <td colSpan={5} className="text-center text-muted">
                          No watchlists Items found.
                        </td>
                      </tr>
                    ) : (
                      watchlistItemList.map((item, index) => (
                        <tr key={item.id}>
                          <th scope="row">{index + 1}</th>
                          <td>{item.id}</td>
                          <td>{item.baseCurrency}</td>
                          <td>{item.quoteCurrency}</td>
                          <td className="d-flex gap-2">
                            <NavLink
                              to={`/alertService/${selectedId}`}
                              className="btn btn-warning"
                              aria-disabled={
                                watchlistItemList.length === 0 ? true : false
                              }
                            >
                              View Alert Detail
                            </NavLink>
                            <button
                              className="btn btn-danger"
                              onClick={() => handleDelete(item.id)}
                              disabled={isDeleting === item.id}
                            >
                              Delete
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

export default ListWatchlistItem;

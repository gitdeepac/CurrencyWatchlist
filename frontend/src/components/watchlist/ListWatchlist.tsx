import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import { watchlistApi } from "../../services/watchlist/api";
import { toast, ToastContainer, type Id } from "react-toastify";

const ListWatchlist = () => {
  const [watchlist, setWatchList] = useState([]); // Start with an empty list
  const [isLoading, setIsLoading] = useState(true); // Loading state
  const [error, setError] = useState(null); // Error state

  const getWatchlist = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const watchListData = await watchlistApi.getAll();
      if (watchListData.success) {
        console.log(watchListData.data);
        setWatchList(watchListData.data || []); // Fallback to empty array if response is null
        setIsLoading(false);
      } else {
        setError(watchListData.message);
        setIsLoading(false);
      }
    } catch (err) {
      setError("Failed to load watchlist. Please try again.");
      setIsLoading(false);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    getWatchlist();
  }, []);

  const handleDelete = async (watchlistId: number) => {
    setIsLoading(true);
    setError(null);
    const prev = watchlist;
    

    // API call
    try {
      var deleteWatchListResp = await watchlistApi.delete(watchlistId);
      
      if (!deleteWatchListResp.success) {
        setWatchList(prev);
        toast.error(deleteWatchListResp.message);
		return;
      }
	  setWatchList((list) => list.filter((w) => w.id !== watchlistId));
	  toast.success(deleteWatchListResp.data.message);
    } catch (err: any) {
      setError(err.response?.data?.message);
      toast.error(err.response?.data?.message);
    } finally {
      setIsLoading(false);
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
              <ToastContainer />
              <table className="table table-striped">
                <thead>
                  <tr>
                    <th scope="col">#</th>
                    <th scope="col">Watchlist Name</th>
                    <th scope="col">Action</th>
                  </tr>
                </thead>
                <tbody>
                  {watchlist.map((item, index) => (
                    <tr key={index}>
                      <th scope="row">{index + 1}</th>
                      <td>{item.name}</td>
                      <td>
                        <button
                          className="btn btn-danger"
                          onClick={() => handleDelete(item.id)}
                        >
                          Delete
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ListWatchlist;

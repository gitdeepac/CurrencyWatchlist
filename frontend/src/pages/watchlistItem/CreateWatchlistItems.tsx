import { useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { watchlistApi } from "../../services/watchlist/api";
import { toast } from "react-toastify";
import { watchlistItemApi } from "../../services/watchlistItem/api";

const CreateWatchlistItems = () => {
  const { watchlistId } = useParams();
  const [formValue, setFormValue] = useState({
    watchlistId: 0,
    baseCurrency: "",
    quoteCurrency: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handelChange = (e) => {
    const { name, value } = e.target;
    setFormValue({
      ...formValue,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.MouseEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    // API call
    try {
      var responseWatchlist = await watchlistItemApi.create(Number(watchlistId), {
        watchlistId: Number(watchlistId),
        baseCurrency: (formValue.baseCurrency).toUpperCase(),
        quoteCurrency: (formValue.quoteCurrency).toUpperCase(),
      });
      toast.success(responseWatchlist.data.message);
    } catch (err: any) {
      setError(err.response?.message);
    } finally {
      setLoading(false);
    }

    // Form Reset
    setFormValue({
      watchlistId: 0,
      baseCurrency: "",
      quoteCurrency: "",
    });
  };

  return (
    <div className="container-fluid">
      <div className="row mt-4">
        <div className="col-12 px-4">
          <div className="card">
            <div className="card-header d-flex justify-content-between">
              <h4 className="m-0">Watchlist - Create</h4>
            </div>
            <div className="card-body">
              {error && <div className="alert alert-danger">{error}</div>}
              <form action="">
                <div className="row">
                  <div className="col-4">
                    <label htmlFor=" ">
                      Base Currency <small>(Max 3 Letters)</small>
                    </label>
                    <input
                      type="text"
                      name="baseCurrency"
                      value={formValue.baseCurrency}
                      placeholder="Please enter base currency Ex: AUD"
                      className="form-control"
                      onChange={handelChange}
                      maxLength={3}
                    />
                  </div>
                  <div className="col-4">
                    <label htmlFor=" ">
                      {" "}
                      Quote Currency <small>(Max 3 Letters)</small>
                    </label>
                    <input
                      type="text"
                      name="quoteCurrency"
                      value={formValue.quoteCurrency}
                      placeholder="Please enter quote currency Ex: AUD"
                      className="form-control"
                      onChange={handelChange}
                      maxLength={3}
                    />
                  </div>
                </div>
                <div className="row mt-4">
                  <div className="col-12 d-flex justify-content-end d-flex gap-2">
                    <button
                      className="btn btn-primary"
                      onClick={handleSubmit}
                      disabled={loading}
                    >
                      {loading ? "Creating..." : "Create WatchlistItems"}
                    </button>
                    <NavLink className="btn btn-danger" to="/watchlist">
                      Cancel
                    </NavLink>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CreateWatchlistItems;

import { useState } from "react";
import { NavLink } from "react-router-dom";
import { watchlistApi } from "../../services/watchlist/api";
import { toast, ToastContainer } from "react-toastify";

const CreateWatchlist = () => {
  const [formValue, setFormValue] = useState({ name: "" });
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
      var responseWatchlist = await watchlistApi.create(formValue);
      toast.success(responseWatchlist.message);
    } catch (err: any) {
      setError(err.response?.data?.title);
    } finally {
      setLoading(false);
    }

    // Form Reset
    setFormValue({
      name: "",
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
              <ToastContainer />
              {error && <div className="alert alert-danger">{error}</div>}
              <form action="">
                <div className="row">
                  <div className="col-12">
                    <label htmlFor=" "> Wathclist Name</label>
                    <input
                      type="text"
                      name="name"
                      value={formValue.name}
                      placeholder="Please enter watchlist"
                      className="form-control"
                      onChange={handelChange}
                    />
                  </div>
                </div>
                <div className="row mt-4">
                  <div className="col-12 d-flex justify-content-end">
                    <button
                      className="btn btn-primary"
                      onClick={handleSubmit}
                      disabled={loading || !formValue.name}
                    >
                      {loading ? "Creating..." : "Create Watchlist"}
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

export default CreateWatchlist;

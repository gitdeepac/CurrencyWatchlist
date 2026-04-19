import { useState } from "react";
import { NavLink } from "react-router-dom";
import { watchlistApi } from "../../services/watchlist/api";
import { toast } from "react-toastify";

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
      toast.success(responseWatchlist.data.message);
      // Form Reset
      setFormValue({ name: "" });
    } catch (err: any) {
      const data = err.response?.data;
      if (data?.errors) {
        const messages = Object.values(data.errors).flat().join(", ");
        setError(messages);
      } else {
        setError(data?.title ?? "Something went wrong."); // fallback
      }
    } finally {
      setLoading(false);
    }
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
                  <div className="col-12 d-flex justify-content-end gap-2">
                    <button
                      className="btn btn-primary"
                      onClick={handleSubmit}
                      disabled={loading || !formValue.name}
                      data-cy="createBtn"
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

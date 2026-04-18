import { useEffect, useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import { alertApi } from "../../services/alert/api";

const CreateListAlert = () => {
  const [formValue, setFormValue] = useState({
    watchlistItemId: 0,
    condition: "",
    threshold: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const { watchlistId, watchlistItemId } = useParams();
  const [selectedWatchListId, setSelectedWatchListId] = useState(() =>
    Number(watchlistId),
  );
  const [selectedWatchListItemId, setSelectedWatchListItemId] = useState(() =>
    Number(watchlistItemId),
  );

  useEffect(() => {
    setSelectedWatchListId(Number(watchlistId));
    setSelectedWatchListItemId(Number(watchlistItemId));
  }, [watchlistId, watchlistItemId]);

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
      var responseAlert = await alertApi.create({
        watchlistItemId: Number(selectedWatchListItemId),
        condition: formValue.condition,
        threshold: formValue.threshold,
      });
      toast.success(responseAlert.data.message);
    } catch (err: any) {
      setError(err.response?.data?.title);
    } finally {
      setLoading(false);
    }

    // Form Reset
    setFormValue({
      watchlistItemId: 0,
      condition: "",
      threshold: "",
    });
  };

  return (
    <div className="container-fluid">
      <div className="row mt-4">
        <div className="col-12 px-4">
          <div className="card">
            <div className="card-header d-flex justify-content-between">
              <h4 className="m-0">Alert - Create</h4>
            </div>
            <div className="card-body">
              {error && <div className="alert alert-danger">{error}</div>}
              <div>
                <div className="row">
                  <div className="col-6">
                    <label htmlFor=" "> Condition</label>
                    <input
                      type="text"
                      name="condition"
                      value={formValue.condition}
                      placeholder="Please enter condition (<, >, =>, =<, = )"
                      className="form-control"
                      onChange={handelChange}
					  maxLength={2}
                    />
                  </div>
                  <div className="col-6">
                    <label htmlFor=" "> Threshold</label>
                    <input
                      type="number"
                      name="threshold"
                      value={formValue.threshold}
                      placeholder="Please enter threshold Ex(114.0, 66.70)"
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
                      disabled={loading}
                    >
                      {loading ? "Creating..." : "Create Alert"}
                    </button>
                    <NavLink
                      className="btn btn-danger"
                      to={`/watchlistItems/${selectedWatchListId}/alertService/${selectedWatchListItemId}`}
                    >
                      Cancel
                    </NavLink>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CreateListAlert;

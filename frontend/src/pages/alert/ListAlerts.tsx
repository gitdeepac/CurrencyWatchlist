import { useEffect, useState } from "react";
import { NavLink, useParams } from "react-router-dom";
import { alertApi } from "../../services/alert/api";
import { toast } from "react-toastify";

interface AlertList {
  watchlistItemId: number;
  condition: string;
  threshold: string;
  isActive: string;
  id: number;
}

const ListAlerts = () => {
  const [alertList, setAlertList] = useState<AlertList[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isDeleting, setIsDeleting] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);
  const { watchlistId, watchlistItemId } = useParams();
  const [selectedWatchListId, setSelectedWatchListId] = useState(
    watchlistId ?? Number(sessionStorage.getItem("selectedWatchListId")),
  );
  const [selectedWatchListItemId, setSelectedWatchListItemId] = useState(
    watchlistItemId ?? Number(sessionStorage.getItem("selectedWatchListItemId")),
  );

  const getAlertList = async (selectedWatchListItemId) => {
    try {
      setIsLoading(true);
      setError(null);
      const alertListData =
        await alertApi.getAlertsByWatchlistItemId(selectedWatchListItemId);
      if (alertListData.success) {
        setAlertList(alertListData.data.data || []);
      } else {
        setError(alertListData.message);
      }
    } catch (err) {
      setError("Failed to load alert list. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    getAlertList(selectedWatchListItemId);
  }, [selectedWatchListItemId]);

  const handleDelete = async (alertId: number) => {
    setIsDeleting(alertId);
    setError(null);

    // API call
    try {
      const deleteWatchListResp = await alertApi.delete(alertId);

      if (!deleteWatchListResp.success) {
        toast.error(deleteWatchListResp.message);
        return;
      }
      setAlertList((list) => list.filter((w) => w.id !== alertId));
      toast.success(deleteWatchListResp.data.message);
    } catch (err: any) {
      setError(err.response?.data?.message);
      toast.error(err.response?.data?.message);
    } finally {
      setIsDeleting(null);
    }
  };

  const handleEvalute = async (wathclistItemsId) => {
    try {
      const evaluate = await alertApi.evaluate(wathclistItemsId);

      if (!evaluate.success) {
        toast.error(evaluate.message);
        return;
      }

      toast.success(evaluate.data.message);

      console.log(evaluate.data);
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
              <h4 className="m-0">Alert - Listing</h4>
              <div className="d-flex gap-2">
                <NavLink
                  to={`/alertService/${selectedWatchListItemId}/add`}
                  className="btn btn-primary"
                >
                  Create Alert
                </NavLink>
                <NavLink to={`/watchlistItems/${selectedWatchListId}`} className="btn btn-primary">
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
                      <th scope="col">Alert Condition</th>
                      <th scope="col">Threshold</th>
                      <th scope="col">Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {alertList.length === 0 ? (
                      <tr>
                        <td colSpan={4} className="text-center text-muted">
                          No alert list found.
                        </td>
                      </tr>
                    ) : (
                      alertList.map((item, index) => (
                        <tr key="">
                          <th scope="row">{index + 1}</th>
                          <td>{item.condition}</td>
                          <td>{item.threshold}</td>
                          <td className="d-flex gap-2">
                            <button
                              className="btn btn-danger"
                              onClick={() => handleEvalute(item.watchlistItemId)}
                              disabled={isDeleting === item.id}
                            >
                              Evaluate
                            </button>
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

export default ListAlerts;

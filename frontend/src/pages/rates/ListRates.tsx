import { useState } from "react";
import { rateApi } from "../../services/rate/api";
import { toast } from "react-toastify";

interface rateList {
  baseCurrency: string;
  quoteCurrency: string;
  rate: string;
  rateDate: string;
}

const RateListService = () => {
  const [ratelist, setRateList] = useState<rateList[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [formValue, setFormValue] = useState({
    baseCurrency: "",
    quoteCurrency: "",
    rate: "",
    rateDate: "",
  });

  const [formValueHistorical, setFormValueHistorical] = useState({
    baseCurrency: "",
    quoteCurrency: "",
    startDate: "",
    endDate: "",
  });


  const handelChangeLatest = (e) => {
    const { name, value } = e.target;
    setFormValue({
      ...formValue,
      [name]: value,
    });

   
  };
  const handelChangeHistory = (e) => {
    const { name, value } = e.target;
     setFormValueHistorical({
      ...formValueHistorical,
      [name]: value,
    });
  };

  const handleRefreshRatesWithInputLatest = async () => {
    try {
      if (formValue.baseCurrency.toUpperCase().length <= 0) {
        toast.error("Please enter base currency");
        return;
      }

      if (formValue.quoteCurrency.toUpperCase().length <= 0) {
        toast.error("Please enter quote currency");
        return;
      }
      setIsLoading(true);
      const refreshRateService = await rateApi.refreshRateWithInput(
        formValue.baseCurrency.toUpperCase(),
        formValue.quoteCurrency.toUpperCase(),
      );

      //console.log(refreshRateService.data.data);
      var item = refreshRateService.data.data;
      const fetchlist = [
        {
          baseCurrency: item.baseCurrency,
          quoteCurrency: item.quoteCurrency,
          rate: item.rate,
          rateDate: item.rateDate,
        },
      ];

      setRateList(fetchlist);
      setFormValue({
        baseCurrency: "",
        quoteCurrency: "",
        rate: "",
        rateDate: "",
      });
      toast.success(refreshRateService.data.message);
    } catch (error) {
      toast.error("Error refreshing rates:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleRefreshRatesWithInputHistorical = async () => {
    if (formValueHistorical.baseCurrency.toUpperCase().length <= 0) {
      toast.error("Please enter base currency");
      return;
    }

    if (formValueHistorical.quoteCurrency.toUpperCase().length <= 0) {
      toast.error("Please enter quote currency");
      return;
    }

    if (formValueHistorical.startDate.length <= 0) {
      toast.error("Please enter quote currency");
      return;
    }
    if (formValueHistorical.endDate.length <= 0) {
      toast.error("Please enter quote currency");
      return;
    }

    try {
      setIsLoading(true);
      const refreshRateService = await rateApi.refreshRateWithInputHistorical(
        formValueHistorical.baseCurrency.toUpperCase(),
        formValueHistorical.quoteCurrency.toUpperCase(),
        formValueHistorical.startDate,
        formValueHistorical.endDate,
      );

      //   console.log(refreshRateService.data.data);
      var item = refreshRateService.data.data;
      const fetchlist = item.map((item: any) => ({
        baseCurrency: item.baseCurrency,
        quoteCurrency: item.quoteCurrency,
        rate: item.rate,
        rateDate: item.rateDate,
      }));

      setRateList(fetchlist);

      toast.success(refreshRateService.data.message);
    } catch (error) {
      toast.error("Error refreshing rates:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container-fluid">
      <div className="row mt-4">
        <div className="col-6 px-4">
          <div className="card">
            <div className="card-header d-flex justify-content-between">
              <h4 className="m-0">Fetch Latest Rate</h4>
            </div>
            <div className="card-body">
              <div className="row">
                <div className="col-6">
                  <label htmlFor=" ">
                    Base Currency <small>(Max 3 Letters)</small>
                  </label>
                  <input
                    type="text"
                    name="baseCurrency"
                    value={formValue.baseCurrency}
                    placeholder="Please enter base currency Ex: AUD"
                    className="form-control"
                    onChange={handelChangeLatest}
                    maxLength={3}
                  />
                </div>
                <div className="col-6">
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
                    onChange={handelChangeLatest}
                    maxLength={3}
                  />
                </div>
              </div>
              <div className="row mt-4">
                <div className="col-12 d-flex justify-content-end d-flex gap-2">
                  <button
                    className="btn btn-primary"
                    onClick={handleRefreshRatesWithInputLatest}
                    disabled={isLoading}
                  >
                    {isLoading ? "Fetching..." : "Fetch Latest Rate "}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-6 px-4">
          <div className="card">
            <div className="card-header d-flex justify-content-between">
              <h4 className="m-0">Fetch Historical Rate</h4>
            </div>
            <div className="card-body">
              <form action="">
                <div className="row">
                  <div className="col-6">
                    <label htmlFor=" ">
                      Base Currency <small>(Max 3 Letters)</small>
                    </label>
                    <input
                      type="text"
                      name="baseCurrency"
                      value={formValueHistorical.baseCurrency}
                      placeholder="Please enter base currency Ex: AUD"
                      className="form-control"
                      onChange={handelChangeHistory}
                      maxLength={3}
                    />
                  </div>
                  <div className="col-6">
                    <label htmlFor=" ">
                      Quote Currency <small>(Max 3 Letters)</small>
                    </label>
                    <input
                      type="text"
                      name="quoteCurrency"
                      value={formValueHistorical.quoteCurrency}
                      placeholder="Start Date (yyyy-mm-dd)"
                      className="form-control"
                      onChange={handelChangeHistory}
                      maxLength={3}
                    />
                  </div>
                  <div className="col-6">
                    <label htmlFor=" ">
                      Start Date <small>YYYY-mm-dd</small>
                    </label>
                    <input
                      type="text"
                      name="startDate"
                      value={formValueHistorical.startDate}
                      placeholder="Start Date (yyyy-mm-dd)"
                      className="form-control"
                      onChange={handelChangeHistory}
                    />
                  </div>
                  <div className="col-6">
                    <label htmlFor=" ">
                      End Date <small>YYYY-mm-dd</small>
                    </label>
                    <input
                      type="text"
                      name="endDate"
                      value={formValueHistorical.endDate}
                      placeholder="End Date (yyyy-mm-dd)"
                      className="form-control"
                      onChange={handelChangeHistory}
                    />
                  </div>
                </div>
                <div className="row mt-4">
                  <div className="col-12 d-flex justify-content-end d-flex gap-2">
                    <button
                      className="btn btn-warning"
                      onClick={handleRefreshRatesWithInputHistorical}
                      disabled={isLoading}
                    >
                      {isLoading ? "Fetching..." : "Fetch Historical Rate "}
                    </button>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
        <div className="col-12 px-4">
          <div className="card">
            
            <div className="card-body">
              {isLoading ? (
                <p className="text-muted">Loading...</p>
              ) : (
                <table className="table table-striped">
                  <thead>
                    <tr>
                      <th scope="col">#</th>
                      <th scope="col">Base Currency</th>
                      <th scope="col">Quote Currency</th>
                      <th scope="col">Rate</th>
                      <th scope="col">Rate Date</th>
                    </tr>
                  </thead>
                  <tbody>
                    {ratelist.length === 0 ? (
                      <tr>
                        <td colSpan={5} className="text-center text-muted">
                          No Rate list found.
                        </td>
                      </tr>
                    ) : (
                      ratelist.map((item, index) => (
                        <tr key={`row_${index}`}>
                          <th scope="row">{index + 1}</th>
                          <td>{item.baseCurrency}</td>
                          <td>{item.quoteCurrency}</td>
                          <td>{item.rate}</td>
                          <td>{item.rateDate}</td>
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

export default RateListService;

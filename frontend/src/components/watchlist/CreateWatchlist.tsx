import { useState } from "react";

// Watchlist Name

const AddWatchlist = () => {
  const [formValue, setFormvalue] = useState({
    watchlistName: "",
  });

  const handelChange = (e) => {
    const { name, value } = e.target;
    setFormvalue({
      ...formValue,
      [name]: value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("form-value", formValue);
    setFormvalue({
      watchlistName: "",
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
              <form action="">
                <div className="row">
                  <div className="col-12">
                    <label htmlFor=" "> Wathclist Name</label>
                    <input
                      type="text"
                      name="watchlistName"
                      value={formValue.watchlistName}
                      placeholder="Please enter watchlist"
                      className="form-control"
                      onChange={handelChange}
                    />
                  </div>
                </div>
                <div className="row mt-4">
                  <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary" onClick={handleSubmit}>
                      Create Watchlist
                    </button>
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

export default AddWatchlist;

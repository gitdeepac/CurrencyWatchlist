describe("Watchlist Items Page", () => {
  it("should load and display watchlist items", () => {
    cy.intercept("GET", "/api/Watchlists/*/items", {
      statusCode: 200,
      body: {
        status: "Success",
        statusCode: 200,
        message: "Successfully Fetch all WatchlistItems",
        data: [
          {
            id: 1,
            watchlistId: 10,
            baseCurrency: "AUD",
            quoteCurrency: "INR",
            createAt: "2026-04-20T22:54:31.903301+10:00",
            latestRate: 66.616,
          },
        ],
      },
    }).as("getWatchlistItems");

    cy.visit("http://localhost:3000/watchlistItems/10");

    cy.wait("@getWatchlistItems");

    cy.contains("Watchlist Item - Listing");
    cy.contains("AUD");
    cy.contains("INR");
    cy.contains("66.616");
  });

  it("should show loading state", () => {
    cy.intercept("GET", "/api/Watchlists/*/items", (req) => {
      req.on("response", (res) => {
        res.setDelay(1000);
      });
    });

    cy.visit("http://localhost:3000/watchlistItems/10");

    cy.contains("Loading...");
  });
  it("should show empty message when no data", () => {
    cy.intercept("GET", "/api/Watchlists/*/items", {
      statusCode: 200,
      body: {
        success: true,
        data: [],
      },
    });

    cy.visit("http://localhost:3000/watchlistItems/10");

    cy.contains("No watchlists Items found.");
  });
  it("should show error message on API failure", () => {
    cy.intercept("GET", "/api/Watchlists/*/items", {
      statusCode: 500,
      body: {
        success: false,
        message: "Server error",
      },
    });

    cy.visit("http://localhost:3000/watchlistItems/10");

    cy.contains("Server error");
  });

  it("should delete a watchlist item", () => {
    cy.intercept("GET", "/api/Watchlists/*/items", {
      statusCode: 200,
      body: {
        success: true,
        data: [
          {
            id: 1,
            watchlistId: 10,
            baseCurrency: "USD",
            quoteCurrency: "AUD",
            latestRate: 1.5,
          },
        ],
      },
    });
	// http://localhost:3000/api/Watchlists/56/items/17
	// DELETE
    cy.intercept("DELETE", "/api/Watchlists/*/items/*", {
      statusCode: 200,
      body: {
        success: true,
        data: { message: "Successfully Deleted Watchlist Item with ID 1" },
      },
    }).as("deleteItem");

    cy.visit("http://localhost:3000/watchlistItems/10");

    cy.contains("Delete").click();

    cy.wait("@deleteItem");

    cy.contains("USD").should("not.exist");
  });
});

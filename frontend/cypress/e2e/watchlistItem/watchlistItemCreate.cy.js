describe("Create Watchlist Page", () => {
  it("should render form and disable button initially", () => {
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
    cy.visit("http://localhost:3000/watchlistItems/10/add");

    cy.contains("Watchlist - Create");

    cy.get("input[name='baseCurrency']").should("exist");
    cy.get("input[name='quoteCurrency']").should("exist");
    cy.get("input[name='quoteCurrency']").should("exist");
    cy.get("input[name='quoteCurrency']").should("exist");
  });

  it("should create watchlist successfully", () => {
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
    cy.intercept("POST", "api/Watchlists/56/items", {
      statusCode: 200,
      body: {
        status: "Success",
        statusCode: 201,
        message: "Successfully Created Watchlist Item.",
        data: {
          id: 1,
          watchlistId: 56,
          baseCurrency: "AUD",
          quoteCurrency: "INR",
          createAt: "2026-04-20T23:21:39.688389+10:00",
          latestRate: null,
        },
      },
    }).as("createWatchlist");

    cy.visit("http://localhost:3000/watchlistItems/56/add");

    cy.get("input[name='baseCurrency']").type("AUD");
    cy.get("input[name='quoteCurrency']").type("INR");

    cy.get(".createWatchlistItemBtn").click();

    cy.wait("@createWatchlist");

    // Form reset
    cy.get("input[name='baseCurrency']").should("have.value", "");
    cy.get("input[name='quoteCurrency']").should("have.value", "");

    // Toast (basic check)
    cy.contains("Successfully Created Watchlist Item.");
  });

  it("should navigate back on cancel", () => {
    cy.visit("http://localhost:3000/watchlistItems/10/add");

    cy.contains("Cancel").click();

    cy.url().should("include", "/watchlist");
  });
});

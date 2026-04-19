describe("Watchlist - List Page", () => {

  beforeEach(() => {
    cy.intercept("GET", "http://localhost:3000/api/watchlists", {
      statusCode: 200,
      body: {
        success: true,
        data: [
          { id: 1, name: "Watchlist One" },
          { id: 2, name: "Watchlist Two" },
        ],
        message: "Successfully fetch list"
      }
    }).as("getWatchlists");

    cy.visit("/watchlist");
    cy.wait("@getWatchlists");
  });

  
  it("should render watchlist table with data", () => {
    cy.contains("Watchlist One").should("be.visible");
    cy.contains("Watchlist Two").should("be.visible");
  });

  
  it("should show empty message when no watchlists", () => {
    cy.intercept("GET", "http://localhost:3000/api/watchlists", {
      statusCode: 200,
      body: { success: true, data: [], message: "Successfully fetch list" }
    }).as("emptyWatchlists");

    cy.visit("/watchlist");
    cy.wait("@emptyWatchlists");

    cy.contains("No watchlists found.").should("be.visible");
  });

  
  it("should navigate to create page", () => {
    cy.get("[data-cy='createWatchlistBtn']").click();
    cy.url().should("include", "/watchlist/add");
  });

  
  it("should disable delete button while deleting", () => {
    cy.intercept("DELETE", "http://localhost:3000/api/watchlists/1", {
      statusCode: 200,
      delay: 1000, 
      body: { success: true, data: { message: "Successfully Deleted Watchlist 1" } }
    }).as("deleteWatchlist");

    cy.get("[data-cy='deleteBtn']").first().click();
    cy.get("[data-cy='deleteBtn']").first().should("have.attr", "disabled");
    cy.wait("@deleteWatchlist");
  });
  
  it("should navigate to watchlist items on view detail", () => {
    cy.contains("View Items Detail").first().click();
    cy.url().should("include", "/watchlistItems/1");
  });

});
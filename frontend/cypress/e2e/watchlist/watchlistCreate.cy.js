describe("Watchlist - CRUD Operations", () => {
  beforeEach(() => {
    cy.visit("/watchlist/add"); 
  });
  it("should create a new watchlist", () => {
    cy.intercept("POST", "http://localhost:3000/api/watchlists", {
      statusCode: 201,
      body: {
        success: true,
        message: "Successfully created watchlist",
        data: { id: 1, name: "Test Watchlist" },
      },
    }).as("createWatchlist");

    cy.visit("/watchlist");

    cy.get("[data-cy='createWatchlistBtn']").click();
    cy.get("[data-cy='createBtn']").should("have.attr", "disabled");
    cy.get("input[name='name']").type("Test Watchlist");
    cy.get("[data-cy='createBtn']").click();

    cy.wait("@createWatchlist", { timeout: 10000 }).then((interception) => {
      expect(interception.response.statusCode).to.eq(201);
    });

    cy.get(".Toastify__toast--success")
      .should("be.visible")
      .and("contain.text", "Successfully created watchlist");
  });

  it("should enable create button when input has value", () => {
    cy.visit("/watchlist/add");
    cy.get("input[name='name']").type("Test Watchlist");
    cy.get("[data-cy='createBtn']").should("not.have.attr", "disabled");
  });

  it("should reset form after successful create", () => {
	cy.visit("/watchlist/add");
    cy.intercept("POST", "http://localhost:3000/api/watchlists", {
      statusCode: 201,
      body: { message: "Successfully created watchlist" },
    }).as("createWatchlist");

    cy.get("input[name='name']").type("Test Watchlist");
    cy.get("[data-cy='createBtn']").click();
    cy.wait("@createWatchlist");

    cy.get("input[name='name']").should("have.value", "");
  });

  it("should navigate back on cancel", () => {
    cy.visit("/watchlist/add");
    cy.get(".btn-danger").click();
    cy.url().should("include", "/watchlist");
  });
});

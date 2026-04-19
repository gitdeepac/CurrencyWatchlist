using Xunit;
using Moq;
using backend.Controllers;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Dtos.Watchlist;

public class WatchlistsControllerTests
{
	private readonly Mock<IWatchlistRepository> _repoMock;
	private readonly WatchlistsController _controller;

	public WatchlistsControllerTests()
	{
		_repoMock = new Mock<IWatchlistRepository>();
		_controller = new WatchlistsController(_repoMock.Object);
	}

	[Fact]
    public async Task GetAll_ReturnsOk_WithWatchlistDtos()
    {
        // Arrange
        var watchlists = new List<Watchlist>
        {
            new Watchlist { Id = 1, Name = "My List" },
            new Watchlist { Id = 2, Name = "Tech Stocks" }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(watchlists);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
    }
	
	[Fact]
	public async Task GetById_ReturnsNotFound_WhenWatchlistMissing()
	{
		// Arrange
		_repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
				 .ReturnsAsync((Watchlist)null);

		// Act
		var result = await _controller.GetById(1);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Fact]
	public async Task Create_ReturnsCreated_WhenValid()
    {
        // Arrange
        var dto = new CreateWatchlistRequestDto { Name = "Valid Name" };
        var createdWatchlist = new Watchlist { Id = 1, Name = "Valid Name" };

        _repoMock.Setup(r => r.CreateWatchlistAsync(It.IsAny<Watchlist>()))
                 .ReturnsAsync(createdWatchlist);  // returns Task<Watchlist>

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
    }
	[Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Name is required");
        var dto = new CreateWatchlistRequestDto();

        // Act
        var result = await _controller.Create(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

	[Fact]
    public async Task Delete_ReturnsOk_WhenWatchlistExists()
    {
        // Arrange
        var watchlist = new Watchlist { Id = 5, Name = "To Delete" };
        _repoMock.Setup(r => r.DeleteWatchlistAsync(5)).ReturnsAsync(watchlist);

        // Act
        var result = await _controller.Delete(5);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
    }

	[Fact]
    public async Task Delete_ReturnsNotFound_WhenWatchlistMissing()
    {
        // Arrange
        _repoMock.Setup(r => r.DeleteWatchlistAsync(It.IsAny<int>()))
                 .ReturnsAsync((Watchlist?)null);

        // Act
        var result = await _controller.Delete(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

}
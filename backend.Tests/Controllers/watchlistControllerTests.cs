using Xunit;
using Moq;
using backend.Controllers;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
		var dto = new CreateWatchlistRequestDto
		{
			Name = "Test"
		};

		_repoMock.Setup(r => r.CreateWatchlistAsync(It.IsAny<Watchlist>()))
				 .Returns(Task.CompletedTask);

		// Act
		var result = await _controller.Create(dto);

		// Assert
		Assert.IsType<CreatedAtActionResult>(result);
	}
}
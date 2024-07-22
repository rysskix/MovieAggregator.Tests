using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAggregator.Controllers;
using MovieAggregator.Data;
using MovieAggregator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieAggregator.Tests
{
    public class ActorsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ActorsController _controller;

        public ActorsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ActorsController(_context);

            _context.Actors.Add(new Actor { FirstName = "Test", LastName = "Actor" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetActors_ReturnsAllActors()
        {
            var result = await _controller.GetActors();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actors = Assert.IsType<List<Actor>>(okResult.Value);
            Assert.Single(actors);
        }

        [Fact]
        public async Task GetActor_ReturnsActor()
        {
            var result = await _controller.GetActor(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actor = Assert.IsType<Actor>(okResult.Value);
            Assert.Equal(1, actor.Id);
        }

        [Fact]
        public async Task PostActor_CreatesActor()
        {
            var actor = new Actor { FirstName = "New", LastName = "Actor" };
            var result = await _controller.PostActor(actor);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdActor = Assert.IsType<Actor>(createdAtActionResult.Value);
            Assert.Equal("New", createdActor.FirstName);
        }

        [Fact]
        public async Task PutActor_UpdatesActor()
        {
            var actor = _context.Actors.First();
            actor.FirstName = "Updated";

            var result = await _controller.PutActor(actor.Id, actor);
            Assert.IsType<NoContentResult>(result);

            var updatedActor = _context.Actors.First();
            Assert.Equal("Updated", updatedActor.FirstName);
        }

        [Fact]
        public async Task DeleteActor_DeletesActor()
        {
            var actor = _context.Actors.First();
            var result = await _controller.DeleteActor(actor.Id);
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(_context.Actors);
        }
    }
}

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
    public class ReviewsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ReviewsController _controller;

        public ReviewsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ReviewsController(_context);

            var movie = new Movie { Title = "Test Movie" };
            _context.Movies.Add(movie);
            _context.SaveChanges();

            _context.Reviews.Add(new Review { Title = "Test Review", Description = "Review Description", Stars = 5, MovieId = movie.Id });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetReviews_ReturnsAllReviews()
        {
            var result = await _controller.GetReviews();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var reviews = Assert.IsType<List<Review>>(okResult.Value);
            Assert.Single(reviews);
        }

        [Fact]
        public async Task GetReview_ReturnsReview()
        {
            var result = await _controller.GetReview(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var review = Assert.IsType<Review>(okResult.Value);
            Assert.Equal(1, review.Id);
        }

        [Fact]
        public async Task PostReview_CreatesReview()
        {
            var review = new Review { Title = "New Review", Description = "New Description", Stars = 4, MovieId = 1 };
            var result = await _controller.PostReview(review);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdReview = Assert.IsType<Review>(createdAtActionResult.Value);
            Assert.Equal("New Review", createdReview.Title);
        }

        [Fact]
        public async Task PutReview_UpdatesReview()
        {
            var review = _context.Reviews.First();
            review.Title = "Updated Review";

            var result = await _controller.PutReview(review.Id, review);
            Assert.IsType<NoContentResult>(result);

            var updatedReview = _context.Reviews.First();
            Assert.Equal("Updated Review", updatedReview.Title);
        }

        [Fact]
        public async Task DeleteReview_DeletesReview()
        {
            var review = _context.Reviews.First();
            var result = await _controller.DeleteReview(review.Id);
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(_context.Reviews);
        }
    }
}

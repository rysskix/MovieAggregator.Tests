using FluentValidation.TestHelper;
using MovieAggregator.Models;
using MovieAggregator.Validator;
using Xunit;

public class MovieValidatorTests
{
    private readonly MovieValidator _validator;

    public MovieValidatorTests()
    {
        _validator = new MovieValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new Movie { Title = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(movie => movie.Title).WithErrorMessage("Title is required");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Specified()
    {
        var model = new Movie { Title = "Valid Title" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(movie => movie.Title);
    }
}

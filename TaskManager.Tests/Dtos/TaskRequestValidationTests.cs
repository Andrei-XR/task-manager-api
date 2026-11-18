using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Dtos;

namespace TaskManager.Tests.Dtos;

public class TaskRequestValidationTests
{
    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void CreateRequest_WithValidData_HasNoErrors()
    {
        var request = new CreateTaskRequest
        {
            Title = "Estudar xUnit",
            Description = "Etapa 18"
        };

        var errors = Validate(request);
    
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateRequest_WithEmptyTitle_IsInvalid(string title)
    {
        var request = new CreateTaskRequest { Title = title! };

        var errors = Validate(request);

        Assert.Contains(errors, e => e.MemberNames.Contains("Title"));
    }

    [Theory]
    [InlineData(200, true)]
    [InlineData(201, false)]
    public void CreateRequest_TitleLength_RespectsLimit(int length, bool shouldBeValid)
    {
        var request = new CreateTaskRequest { Title = new string('a', length) };

        var errors = Validate(request);

        Assert.Equal(shouldBeValid, errors.Count == 0);
    }

    [Theory]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public void CreateRequest_DescriptionLength_RespectsLimit(int length, bool shouldBeValid)
    {
        var request = new CreateTaskRequest 
        { 
            Title = "Titulo",
            Description = new string('a', length)
        };

        var errors = Validate(request);

        Assert.Equal(shouldBeValid, errors.Count == 0);
    }

    [Fact]
    public void UpdateRequest_WithEmptyTitle_IsInvalid()
    {
        var request = new UpdateTaskRequest { Title = "", IsCompleted = true };

        var errors = Validate(request);

        Assert.Contains(errors, e => e.MemberNames.Contains("Title"));
    }
}
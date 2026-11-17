using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Dtos;

public class CreateTaskRequest
{
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Description { get; set; }
}

public class UpdateTaskRequest
{
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}

public class UpdateTaskStatusRequest
{
    public bool IsCompleted { get; set; }
}
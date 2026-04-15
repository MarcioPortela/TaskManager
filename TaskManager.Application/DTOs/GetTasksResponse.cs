namespace TaskManager.Application.DTOs;

public class GetTasksResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
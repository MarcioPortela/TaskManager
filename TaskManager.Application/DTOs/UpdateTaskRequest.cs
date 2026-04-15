namespace TaskManager.Application.DTOs
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
    }
}

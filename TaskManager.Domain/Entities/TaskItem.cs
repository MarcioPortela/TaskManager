using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public Enums.TaskStatus Status { get; private set; }

        public TaskItem(string title, Enums.TaskStatus status, string? description = null, DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título é obrigatório.", nameof(title));

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
        }

        public void Update(string title, string? description, DateTime? dueDate, Enums.TaskStatus status)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título é obrigatório.", nameof(title));

            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
        }
    }
}

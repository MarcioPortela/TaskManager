namespace TaskManager.Application.DTOs
{
    public class CreateTaskResponse
    {
        public Guid Id { get; set; }

        public CreateTaskResponse(Guid id)
        {
            Id = id;
        }
    }
}

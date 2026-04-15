namespace TaskManager.Application.DTOs
{
    /// <summary>
    /// Representa os dados necessários para criar uma nova tarefa.
    /// </summary>
    public class CreateTaskRequest
    {
        /// <summary>
        /// O título principal da tarefa. (Obrigatório)
        /// </summary>
        /// <example>Revisar Pull Requests</example>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes adicionais sobre a tarefa. (Opcional)
        /// </summary>
        /// <example>Verificar se a feature foi construida seguindo o padrão do projeto.</example>
        public string? Description { get; set; }

        /// <summary>
        /// Data limite para a conclusão da tarefa. (Opcional)
        /// </summary>
        /// <example>2026-05-20T14:30:00Z</example>
        public DateTime? DueDate { get; set; }
    }
}


namespace DeltaX.DemoServerTodoAppV1.Dtos
{
    public record TodoCreateDto
    { 
        public string Description { get; set; }
        public bool Completed { get; set; } = false;
    }
}

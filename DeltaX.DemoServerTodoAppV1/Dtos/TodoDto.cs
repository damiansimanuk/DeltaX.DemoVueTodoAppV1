namespace DeltaX.DemoServerTodoAppV1.Dtos
{
    using System;

    public record TodoDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}

﻿namespace DeltaX.DemoServerTodoAppV1.Dtos
{
    public record TodoUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; } 
    }
}

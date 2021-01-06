namespace DeltaX.DemoServerTodoAppV1.Dtos
{
    using System;

    public class GetTodoSinceRequestDto
    {
        public DateTimeOffset? Since { get; set; }
        public int Timeout { get; set; } = 60; 
    }
}

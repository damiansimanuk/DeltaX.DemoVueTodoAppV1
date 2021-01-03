using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaX.DemoServerTodoAppV1.Dtos
{
    public record TodoUpdateDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaX.DemoServerTodoAppV1.Dtos
{
    public record TodoDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool Completed { get; set; }
    }
}

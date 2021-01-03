using DeltaX.DemoServerTodoAppV1.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeltaX.DemoServerTodoAppV1.Repositories
{
    public interface ITodoRepository
    {
        TodoDto Get(Guid id);
        PaginatingResultDto<TodoDto> GetAll(int skipCount = 0, int? maxResultCount = 10);
        TodoDto Insert(TodoCreateDto item);
        IEnumerable<TodoDto> InsertMany(IEnumerable<TodoCreateDto> items);
        TodoDto Remove(Guid id);
        TodoDto Update(TodoUpdateDto item);
    }
}

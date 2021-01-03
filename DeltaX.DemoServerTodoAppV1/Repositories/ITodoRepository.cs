namespace DeltaX.DemoServerTodoAppV1.Repositories
{
    using DeltaX.DemoServerTodoAppV1.Dtos;

    public interface ITodoRepository
    {
        TodoDto Get(int id);
        PaginatingResultDto<TodoDto> GetAll(int skipCount = 0, int? maxResultCount = 10);
        TodoDto Insert(TodoCreateDto item); 
        TodoDto Remove(int id);
        TodoDto Update(TodoUpdateDto item);
    }
}

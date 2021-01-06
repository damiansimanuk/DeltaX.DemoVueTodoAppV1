namespace DeltaX.DemoServerTodoAppV1.Repositories
{
    using Dapper;
    using DeltaX.DemoServerTodoAppV1.Dtos;
    using DeltaX.DemoServerTodoAppV1.Repositories.Sqlite; 
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;

    public class TodoSqliteRepository : ITodoRepository
    {
        private readonly IDbConnection db;
        private readonly ILogger<TodoRepository> logger;

        public TodoSqliteRepository(IDbConnection db, ILogger<TodoRepository> logger = null)
        {
            this.db = db;
            this.logger = logger;
        }

        public TodoDto Get(int id)
        {
            return db.QueryFirstOrDefault<TodoDto>(SqlQueries.GetTodoById, new { Id = id });
        }

        public PaginatingResultDto<TodoDto> GetAll(int skipCount = 0, int? maxResultCount = 10)
        {
            var total = db.ExecuteScalar<int>(SqlQueries.CountTodo);
            maxResultCount = maxResultCount ?? total;

            var query = SqlQueries.GetTodosPaged
              .Replace("{RowsPerPage}", $"{maxResultCount}")
              .Replace("{SkipCount}", $"{skipCount}");

            var resItems = db.Query<TodoDto>(query);

            return new PaginatingResultDto<TodoDto>(total, maxResultCount.Value, skipCount, resItems);
        }


        public TodoDto Insert(TodoCreateDto item)
        {
            var now = DateTime.Now;
            var id = db.ExecuteScalar<int>(SqlQueries.InsertTodo, new
            {
                Description = item.Description,
                Completed = item.Completed,
                Updated = now,
                Created = now
            });
            return Get(id);
        }

        public TodoDto Remove(int id)
        {
            var item = Get(id);
            if (item != null)
            {
                db.Execute(SqlQueries.DeleteTodoById, new { Id = id });
            }
            return item;
        }

        public TodoDto Update(TodoUpdateDto item)
        {
            logger?.LogInformation("Update DB id:{0}", item.Id);

            db.Execute(SqlQueries.UpdateTodo, new
            {
                Id = item.Id,
                Description = item.Description,
                Completed = item.Completed,
                Updated = DateTime.Now
            });
            return Get(item.Id);
        }
    }
}

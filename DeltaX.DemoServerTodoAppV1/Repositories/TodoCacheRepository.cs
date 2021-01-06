namespace DeltaX.DemoServerTodoAppV1.Repositories
{
    using Dapper;
    using DeltaX.DemoServerTodoAppV1.Dtos;
    using DeltaX.DemoServerTodoAppV1.Repositories.Sqlite;
    using DeltaX.DemoServerTodoAppV1.Repositories.Tracker;
    using DynamicData;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TodoCacheRepository: TodoSqliteRepository, ITodoRepository
    {
        private readonly ITodoCache todoCache;

        public TodoCacheRepository(
            ITodoCache todoCache,
            IDbConnection db,
            ILogger<TodoRepository> logger = null)
            : base(db, logger)
        {
            this.todoCache = todoCache;

            // Initialize Cache with all items
            if (this.todoCache.SourceCache.Count == 0)
            {
                this.todoCache.SourceCache.AddOrUpdate(base.GetAll(0, 1000).Items);
            }
        }

        public new TodoDto Insert(TodoCreateDto item)
        {
            var inserted = base.Insert(item);
            todoCache.SourceCache.AddOrUpdate(inserted);
            return inserted;
        }

        public new TodoDto Remove(int id)
        {
            var item = base.Remove(id);
            if (item != null)
            {
                todoCache.SourceCache.RemoveKey(item.Id);
            }
            return item;
        }

        public new TodoDto Update(TodoUpdateDto item)
        {
            var updated = base.Update(item);
            todoCache.SourceCache.AddOrUpdate(updated);
            return updated;
        }  
    }
}

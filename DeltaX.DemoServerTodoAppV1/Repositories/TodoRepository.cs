namespace DeltaX.DemoServerTodoAppV1.Repositories
{
    using DeltaX.DemoServerTodoAppV1.Dtos;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TodoRepository : ITodoRepository
    {
        private static int idCounter = 1;
        private List<TodoDto> items;
        private ILogger<TodoRepository> logger;

        public TodoRepository(ILogger<TodoRepository> logger = null)
        {
            items = new List<TodoDto>();
            this.logger = logger;
        }

        public TodoDto Get(int id)
        {
            return items.FirstOrDefault(i => i.Id == id);
        }

        public PaginatingResultDto<TodoDto> GetAll(int skipCount = 0, int? maxResultCount = 10)
        {
            var total = items.Count();
            maxResultCount = maxResultCount ?? total;

            var resItems = items.Skip(skipCount).Take(maxResultCount.Value); 

            return new PaginatingResultDto<TodoDto>(total, maxResultCount.Value, skipCount, resItems);
        }


        public TodoDto Insert(TodoCreateDto item)
        {
            var clone = new TodoDto
            {
                Id = idCounter++,
                Description = item.Description,
                Completed = item.Completed,
                Created = DateTimeOffset.Now,
                Updated = DateTimeOffset.Now
            };

            items.Add(clone);
            return clone;
        }

        public IEnumerable<TodoDto> InsertMany(IEnumerable<TodoCreateDto> items)
        {
            var clones = items.Select(i =>
            {
                return new TodoDto
                {
                    Id = idCounter++,
                    Description = i.Description,
                    Completed = i.Completed,
                    Created = DateTimeOffset.Now,
                    Updated = DateTimeOffset.Now
                };
            }).ToList();

            this.items.AddRange(clones);
            return clones;
        }

        public TodoDto Remove(int id)
        {
            var item = Get(id);
            if (item != null)
            {
                items.Remove(item);
            }
            return item;
        }

        public TodoDto Update(TodoUpdateDto item)
        {
            logger?.LogInformation("Update DB id:{0}", item.Id);
            var prevItem = Remove(item.Id);
            if (prevItem != null)
            {
                var clone = prevItem with { Description = item.Description, Completed = item.Completed, Updated = DateTimeOffset.Now };

                items.Add(clone);
                return clone;
            }
            return null;
        }
    }
}

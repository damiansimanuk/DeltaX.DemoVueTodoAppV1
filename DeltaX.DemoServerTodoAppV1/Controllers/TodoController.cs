namespace DeltaX.DemoServerTodoAppV1.Controllers
{
    using DeltaX.DemoServerTodoAppV1.Dtos;
    using DeltaX.DemoServerTodoAppV1.Repositories;
    using DeltaX.DemoServerTodoAppV1.Repositories.Tracker;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository repository;
        private readonly ITodoCache cache;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoRepository repository, ITodoCache cache, ILogger<TodoController> logger)
        {
            this.repository = repository;
            this.cache = cache;
            _logger = logger;
        }

        [HttpGet("Items")]
        public PaginatingResultDto<TodoDto> GetAll(
             [FromQuery] int skipCount = 0,
             [FromQuery] int? maxResultCount = 10)
        { 
            return repository.GetAll(skipCount, maxResultCount);
        }

        [HttpGet("Item/{id}")]
        public TodoDto Get(int id)
        {
            return repository.Get(id);
        }

        [HttpPut("Item/{id}")]
        public TodoDto Update(int id, TodoUpdateDto item)
        {
            if (id != item.Id)
            {
                throw new ArgumentException("Invalid Id", nameof(id));
            }
            return repository.Update(item);
        }

        [HttpPost("Items")]
        public TodoDto InsertSingle(
            [FromBody] TodoCreateDto newItemDto)
        {
            return repository.Insert(newItemDto);
        }

        [HttpDelete("Item/{id}")]
        public TodoDto Remove(int id)
        {
            return repository.Remove(id);
        }

        [HttpGet("/test")]
        public IEnumerable<TodoDto> GetTest()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new TodoDto
            {
                Created = DateTime.Now.AddDays(index),
                Description = $"Hola mundo {index}"
            })
            .ToArray();
        }

        [HttpPost("Items/GetSince")]
        public Task<DataTrackerResultDto<TodoDto>> GetSince(
           CancellationToken cancellation,
           [FromBody] GetTodoSinceRequestDto getSince
           )
        {
            var since = getSince.Since ?? new DateTimeOffset(DateTime.Now); 
            var timeout = TimeSpan.FromSeconds(getSince.Timeout);

            bool filter(DataTracker<TodoDto> i) => i.Updated > since;

            return cache.GetChangeItemsAsync(filter, timeout, cancellation);
        }
    }
}

namespace DeltaX.DemoServerTodoAppV1.Repositories
{
    using DeltaX.DemoServerTodoAppV1.Dtos;
    using DeltaX.DemoServerTodoAppV1.Repositories.Tracker;
    using DynamicData;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITodoCache
    {
        SourceCache<TodoDto, int> SourceCache { get; }
        IObservableCache<DataTracker<TodoDto>, int> SharedCache { get; }

        Task<DataTrackerResultDto<TodoDto>> GetChangeItemsAsync(
            Func<DataTracker<TodoDto>, bool> filter,
            TimeSpan timeout,
            CancellationToken? cancellation = null);
    }
}

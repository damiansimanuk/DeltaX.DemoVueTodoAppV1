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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TodoCache : ITodoCache
    {
        private readonly ILogger logger;

        public TodoCache(ILogger logger, TimeSpan? expireAfter = null, int? limitSizeTo = null)
        {
            this.logger = logger;
            this.SourceCache = new SourceCache<TodoDto, int>(t => t.Id);

            // Expiration time
            if (expireAfter.HasValue)
            {
                SourceCache.ExpireAfter(u => expireAfter.Value, TimeSpan.FromSeconds(expireAfter.Value.TotalSeconds / 10))
                    .Subscribe(x => logger.LogInformation("SourceCache Expiration: {0} filled trades have been removed from memory", x.Count()));
            }

            // Limit size
            if (limitSizeTo.HasValue)
            {
                SourceCache.LimitSizeTo(limitSizeTo.Value)
                    .Subscribe(x => logger.LogInformation("SourceCache LimitSize: {0} filled trades have been removed from memory", x.Count()));
            }

            // Shared object
            SharedCache = SourceCache.Connect()
                .WhereReasonsAre(ChangeReason.Add, ChangeReason.Moved, ChangeReason.Refresh, ChangeReason.Update, ChangeReason.Remove)
                .Transform(w => new DataTracker<TodoDto>(w, null))
                .ForEachChange(e =>
                {
                    e.Current.Reason = e.Reason;
                    e.Current.Updated = DateTime.Now;
                })
                .AsObservableCache();
        }

        public SourceCache<TodoDto, int> SourceCache { get; private set; }

        public IObservableCache<DataTracker<TodoDto>, int> SharedCache { get; private set; }

        public async Task<DataTrackerResultDto<TodoDto>> GetChangeItemsAsync(
            Func<DataTracker<TodoDto>, bool> filter, TimeSpan timeout, CancellationToken? cancellation)
        {
            var locker = new object(); 
            var removes = SharedCache.Connect()
                .WhereReasonsAre(ChangeReason.Remove)
                .Synchronize(locker);

            var notRemoved = SharedCache.Connect()
                .WhereReasonsAreNot(ChangeReason.Remove)
                .Filter(filter)
                .Synchronize(locker);

            var res = await notRemoved.Merge(removes)
                .GetItemsAsync(timeout, cancellation);
            return new DataTrackerResultDto<TodoDto>(res);
        }
    }
}

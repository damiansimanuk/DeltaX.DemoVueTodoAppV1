namespace DeltaX.DemoServerTodoAppV1.Repositories.Tracker
{
    using DynamicData;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class DynamicDataExtension
    {
        public static Task<List<TResult>> GetItemsAsync<TResult>(
            this IObservable<IChangeSet<TResult>> source,
            TimeSpan timeout,
            CancellationToken? cancellationToken = null)
        {
            cancellationToken = cancellationToken ?? CancellationToken.None;

            return Task.Run(() =>
            {
                var result = new List<TResult>();
                using (var waitData = new ManualResetEventSlim(false))
                using (var sub = source
                    .Do(c =>
                    {
                        result = c.Select(i => i.Item.Current).ToList();
                        waitData.Set();
                    })
                    .Subscribe())
                {
                    waitData.Wait(timeout, cancellationToken.Value);
                    return result;
                }
            }, cancellationToken.Value);
        }
         
        public static Task<List<TResult>> GetItemsAsync<TResult, TKey>(
            this IObservable<IChangeSet<TResult, TKey>> source,
            TimeSpan timeout,
            CancellationToken? cancellationToken = null)
        {
            cancellationToken = cancellationToken ?? CancellationToken.None;

            return Task.Run(() =>
            {
                var result = new List<TResult>();
                using (var waitData = new ManualResetEventSlim(false))
                using (var sub = source
                    .Do(c =>
                    {
                        result = c.Select(i => i.Current).ToList();
                        waitData.Set();
                    })
                    .Subscribe())
                {
                    waitData.Wait(timeout, cancellationToken.Value);
                    return result;
                }
            }, cancellationToken.Value);
        }
    }
}
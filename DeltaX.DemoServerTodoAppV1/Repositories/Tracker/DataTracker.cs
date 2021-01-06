namespace DeltaX.DemoServerTodoAppV1.Repositories.Tracker
{
    using DynamicData;
    using System;

    public class DataTracker<TItem>
    {
        public DateTimeOffset Updated { get; set; }
        public ChangeReason Reason { get; set; }
        public TItem Item { get; set; }
        public DataTracker(TItem item, DateTimeOffset? updated = null)
        {
            this.Updated = updated ?? new DateTimeOffset(DateTime.Now);
            this.Item = item;
        }
    }
}

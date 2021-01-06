namespace DeltaX.DemoServerTodoAppV1.Repositories.Tracker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DataTrackerResultDto<TItem>
    {
        public DataTrackerResultDto(IEnumerable<DataTracker<TItem>> items)
        {
            if (items != null && items.Any())
            {
                First = items.Min(i => i.Updated);
                Last = items.Max(i => i.Updated);
                Items = items.ToArray();
            }
        }

        public IEnumerable<DataTracker<TItem>> Items { get; set; }

        public DateTimeOffset? First { get; set; }
        public DateTimeOffset? Last { get; set; }
    }
}

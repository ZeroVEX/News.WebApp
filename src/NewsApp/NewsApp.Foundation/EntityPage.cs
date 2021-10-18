using System.Collections.Generic;

namespace NewsApp.Foundation
{
    public class EntityPage<T>
    {
        public IReadOnlyCollection<T> Items { get; }

        public int Count { get; }


        public EntityPage(IReadOnlyCollection<T> items, int count)
        {
            Items = items;
            Count = count;
        }
    }
}
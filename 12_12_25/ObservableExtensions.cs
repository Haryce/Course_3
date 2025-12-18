using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace StoreG1G3.Extensions
{
    public static class ObservableExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
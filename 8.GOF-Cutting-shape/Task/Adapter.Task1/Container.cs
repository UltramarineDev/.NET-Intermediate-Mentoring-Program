using System.Collections.Generic;
using System.Linq;

namespace Adapter.Task1
{
    public class Container<T>: IContainer<T>
    {
        public Container(IEnumerable<T> items)
        {
            Items = items;
        }
        
        public IEnumerable<T> Items { get; }
        public int Count => Items.Count();
    }
}

using System.Collections.Generic;

namespace Adapter.Task1
{
    //can not change
    public interface IElements<T>
    {
        IEnumerable<T> GetElements();
    }
}

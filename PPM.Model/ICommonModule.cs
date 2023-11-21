using System.Collections.Generic;

namespace PPM.Model
{
    public interface ICommonModule<T>
    {
        void Add(T item);
        List<T> GetItems();
        T GetItemById(int id);
        void Delete(int id);
        void ViewItems();
        void UpdateItem(T updatedItem);
    }
}

namespace Project4.Models
{
    public interface IListOfObjects<T>
    {
        //Add a new object to the list
        void Add(T obj);
        //remove an item from teh list at index
        void RemoveAtIndex(int index);
    }
}

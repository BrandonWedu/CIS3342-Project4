namespace Project4.Models
{
    public class ListOfObjects<T> : IListOfObjects<T> where T : ICloneable<T>
    {
        protected List<T> list = new List<T>();

        public List<T> List
        {
            get { return ListClone(); }
            set
            {
                list.Clear();
                foreach (T item in value)
                {
                    Add(item.Clone());
                }
            }
        }

        //Implement Interface
        public void Add(T obj)
        {
            list.Add(obj.Clone());
        }
        public void RemoveAtIndex(int index)
        {
            if (index > -1 && index < list.Count)
            {
                list.RemoveAt(index);
            }
        }

        protected List<T> ListClone()
        {
            List<T> temp = new List<T>();
            foreach (T obj in list)
            {
                temp.Add(obj.Clone());
            }
            return temp;
        }
    }
}

namespace Project4.Models
{
    public class ListOfObjects<T> : IListOfObjects<T> where T : ICloneable<T>
    {
        protected List<T> list = new List<T>();

        public List<T> List
        {
            get { return ListDeepCopy(); }
            set
            {
                list.Clear();
                foreach (T item in value)
                {
                    Add(item);
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

        public List<T> ListDeepCopy()
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

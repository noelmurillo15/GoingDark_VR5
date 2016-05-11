using System;
using System.Collections;
using System.Collections.Generic;

namespace AGS.Core.Classes.ActionProperties
{   
    /// <summary>
    /// A list with extra observable functionality.
    /// Add subscription methods to Actions ListItemAdded, ListItemRemoved and ListCleared to subscribe to corresponding events
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionList<T> : IList<T>
    {
        public Action<T> ListItemAdded { get; set; }
        public Action<T> ListItemRemoved { get; set; }
        public Action ListCleared { get; set; }
        private readonly List<T> _list = new List<T>();
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _list.Add(item);
            if (ListItemAdded != null)
            {
                ListItemAdded(item);
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            _list.AddRange(collection);
        }

        public void Reverse()
        {
            _list.Reverse();
        }
        public void Clear()
        {
            _list.Clear();
            if (ListCleared != null)
            {
                ListCleared();
            }
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (ListItemRemoved != null)
            {
                ListItemRemoved(item);
            }
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly { get { return false; } }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }

    public static class ListExtensions
    {
        public static ActionList<T> ToActionList<T>(this IList<T> ilist)
        {
            var actionList = new ActionList<T>();
            actionList.AddRange(ilist);
            return actionList;
        }
    }


}

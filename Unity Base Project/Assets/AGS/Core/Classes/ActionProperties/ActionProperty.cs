using System;
using UnityEngine;

namespace AGS.Core.Classes.ActionProperties
{
    /// <summary>
    /// A property with extra subscription functionality.
    /// Add a subscription method to Action OnValueChanged to detect when property value is changed.
    /// New subscribers will automatically get an initial notification of the current value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionProperty<T>
    {
        [SerializeField]
        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == null)
                {
                    if (_value == null) return;
                    _value = default(T);
                    if (SubscriberEvent != null)
                    {
                        SubscriberEvent(this, new ActionPropertyEventArgs<T> { Value = default(T) });
                    }
                }
                else
                {
                    if (_value != null && _value.Equals(value)) return;
                    _value = value;

                    if (SubscriberEvent != null)
                    {
                        SubscriberEvent(this, new ActionPropertyEventArgs<T> { Value = _value });
                    }
                }
            }
        }

        event EventHandler<ActionPropertyEventArgs<T>> SubscriberEvent;

        readonly object _objectLock = new System.Object();

        public event EventHandler<ActionPropertyEventArgs<T>> OnValueChanged
        {
            add
            {
                lock (_objectLock)
                {
                    SubscriberEvent += value;
                    if (_value != null)
                    {
                        value.Invoke(this, new ActionPropertyEventArgs<T> { Value = _value });
                    }
                }
            }
            remove
            {
                lock (_objectLock)
                {
                    if (value == null) return;
                    SubscriberEvent -= value;
                }
            }
        }
    }

    public class ActionPropertyEventArgs<T> : EventArgs
    {
        public T Value { get; set; }
    }
}

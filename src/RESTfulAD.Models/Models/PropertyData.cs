using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System;

namespace RESTfulAD.Models
{
    public class PropertyData : CollectionBase
    {
        protected PropertyData(int capacity) : base(capacity)
        {
        }

        public PropertyData()
        {
        }

        public PropertyData(object value)
        {
            ICollection objs = value as ICollection;

            if (value != null && !(value is ResultPropertyValueCollection) && !(value is PropertyValueCollection))
            {
                objs = new List<object>(new[] { value });
            }

            foreach (var item in objs?.Cast<object>() ?? Enumerable.Empty<object>())
            {
                List.Add(item);
            }
        }

        public object Value
        {
            get
            {
                object _return = null;
                if (Count == 1)
                    _return = List[0];

                if (Count > 1)
                    _return = List.Cast<object>().ToArray();

                return _return;
            }
        }

        public override string ToString()
        {
            var value = Value;
            if(value is Array)
            {
                return string.Join(";", value as Array);
            }
                        
            return Value?.ToString();
        }

        public List<object> Values
        {
            get
            {
                return List.Cast<object>().ToList();
            }
        }

        public object this[int index]
        {
            get
            {
                if (index >= this.Count) return null;

                return List[index];
            }
        }

        internal static PropertyData FromValue(object value)
        {
            return new PropertyData(value);
        }
    }
}
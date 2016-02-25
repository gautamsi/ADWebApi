using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace RESTfulAD.Models
{
    public class PropertyCache : IDictionary<string, PropertyData>
    {
        public PropertyCache(SearchResult sr) : this()
        {
            SearchResult = sr;
            //FromIDictionary(_sr.Properties);
        }

        public PropertyCache(DirectoryEntry de) : this()
        {
            DirectoryEntry = de;
            //FromIDictionary(_de.Properties);
        }

        public PropertyCache()
        {
            //_dict = new Dictionary<string, object>();
            //DirectoryEntry = new DirectoryEntry();// { UsePropertyCache = true };
        }

        //private Dictionary<string, object> _dict;
        internal SearchResult SearchResult { get; private set; }

        internal DirectoryEntry DirectoryEntry { get; private set; }

        public bool Contains(string key)
        {
            return _props.Contains(key.ToLower());
        }

        public bool ContainsKey(string key)
        {
            return this.Contains(key);
        }

        public void Add(string key, PropertyData value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out PropertyData value)
        {
            value = null;

            if (this.Contains(key))
            {
                value = this[key];
                return true;
            }

            return false;
        }

        public void Add(KeyValuePair<string, PropertyData> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, PropertyData> item)
        {
            return this[item.Key] == item.Value;
        }

        public void CopyTo(KeyValuePair<string, PropertyData>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, PropertyData> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, PropertyData>> GetEnumerator()
        {
            foreach (DictionaryEntry item in _props)
            {
                yield return new KeyValuePair<string, PropertyData>(item.Key.ToString(), item.Value.AsPropertyData());
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public ICollection PropertyNames { get { return _props.Keys; } }

        public ICollection<string> Keys
        {
            get
            {
                return (ICollection<string>)_props.Keys;
            }
        }

        public ICollection<PropertyData> Values
        {
            get
            {
                return _props.Values.Cast<PropertyData>().ToList();
            }
        }

        public int Count
        {
            get
            {
                return _props.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public PropertyData this[string key]
        {
            get
            {
                if (String.Compare("ADsPath", key, true) == 0 && DirectoryEntry?.Path != null)
                {
                    return new PropertyData(DirectoryEntry.Path);
                }
                return DirectoryEntry?.Properties?[key]?.AsPropertyData() ?? SearchResult?.Properties?[key]?.AsPropertyData();
                //return _props?[key].AsPropertyData();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        private void FromIDictionary(IDictionary dict)
        {
            foreach (IDictionaryEnumerator item in dict)
            {
                throw new NotImplementedException();
                //_dict.Add(item.Key.ToString(), item.Value);
            }
        }

        private IDictionary _props
        {
            get
            {
                if (this.DirectoryEntry != null)
                    return DirectoryEntry.Properties;

                return SearchResult?.Properties;
            }
        }
    }
}
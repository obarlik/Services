using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utilities
{
    public class DictionaryCache<T> : Dictionary<string, T>
    {
        public DictionaryCache(Func<string, T> initializer)
        {
            Initializer = initializer;
        }
        

        Func<string, T> Initializer { get; }


        public new T this[string key]
        {
            get
            {
                if (ContainsKey(key))
                    return base[key];

                lock (this)
                {
                    if (ContainsKey(key))
                        return base[key];

                    var result = Initializer(key);

                    base[key] = result;

                    return result;
                }
            }

            set { lock (this) { base[key] = value; } }
        }        
    }

}

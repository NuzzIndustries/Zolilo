using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zolilo.Web
{
    /// <summary>
    /// Represents a class that contains a key value store with a specified context
    /// </summary>
    public class ZoliloStateTable : Dictionary<string, object>
    {
        internal new object this[string key]
        {
            get
            {
                if (this.ContainsKey(key))
                    return base[key];
                return null;
            }
            set
            {
                if (!this.ContainsKey(key))
                    Add(key, value);
                else
                    base[key] = value;
            }
        }

        /// <summary>
        /// Generates a random key and returns the value
        /// </summary>
        /// <param name="keyBase">the specified prefix of the key</param>
        /// <returns>The key generated</returns>
        internal int GenerateKey(string keyBase)
        {
            Random r = new Random();
            int randomInteger = 0;
            do
            {
                randomInteger = r.Next();
            }
            while (randomInteger < 1 || this.ContainsKey(keyBase + randomInteger.ToString()));
            return randomInteger;
        }
    }
}

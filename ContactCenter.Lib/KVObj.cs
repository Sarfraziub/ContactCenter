using System;
using System.Collections.Generic;
using System.Text;

namespace ContactCenter.Lib
{
    public class KVObj<T, V>
    {
        public T Key { get; set; }
        public V Value { get; set; }
    }
}

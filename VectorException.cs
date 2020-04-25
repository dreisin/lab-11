using System;
using System.Collections.Generic;
using System.Text;

namespace Task11
{
    class VectorException : Exception
    {
       public VectorException(string message) : 
        base(message) { }
    }
}

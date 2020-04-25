using System;
using System.Collections.Generic;
using System.Text;

namespace Task11
{
    class ComplexNumberException: ArithmeticException
    {
        public ComplexNumberException(string message): 
            base(message)
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Task11
{
    struct ComplexPair
    {
        public ComplexNumber First { get; set; }
        public ComplexNumber Second { get; set; }

        public ComplexPair(ComplexNumber first, ComplexNumber second)
        {
            First = first;
            Second = second;
        }
    }
}

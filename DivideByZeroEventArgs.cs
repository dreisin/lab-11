using System;
using System.Collections.Generic;
using System.Text;

namespace Task11
{
    class DivideByZeroEventArgs : EventArgs
    {
        public ComplexNumber Division { get; set; }
        public ComplexNumber Divider { get; set; }
    }
}

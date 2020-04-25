using System;
using System.Collections.Generic;

namespace Task11
{
    class Program
    {
        static void Main(string[] args)
        {
            ComplexNumber num1 = new ComplexNumber(2.0, 3.0);
            ComplexNumber num2 = new ComplexNumber(1.0, 4.0);
            ComplexNumber num3 = new ComplexNumber(5.0, 3.0);
            ComplexNumber num4 = new ComplexNumber(3.0, 7.0);
            ComplexNumber num5 = new ComplexNumber(4.0, 0.0);


            ComplexNumber num6 = new ComplexNumber(3);
            Vector<ComplexNumber> vec1 = new Vector<ComplexNumber>(new ComplexNumber[] { num1, num2 });
            Vector<ComplexNumber> vec2 = new Vector<ComplexNumber>(new ComplexNumber[] { num3, num4 });

            num4.DivideByZero += func;
            Vector<int> vc = new Vector<int>(new int[] { 0 });
            Console.WriteLine(num1.Module);
            Console.WriteLine(vec1 + vec2);
            Console.WriteLine("{0}", ComplexNumber.sqrt(num5, 2));
        }
        static public void func(object obj, DivideByZeroEventArgs args)
        {
            Console.WriteLine("In func");
        }
    }
}

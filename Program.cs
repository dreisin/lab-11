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
            ComplexNumber num5 = new ComplexNumber(0.0, 0.0);


            ComplexNumber num6 = new ComplexNumber(3);
            Vector<ComplexNumber> vec1 = new Vector<ComplexNumber>(new ComplexNumber[] { num1, num2 });
            Vector<ComplexNumber> vec2 = new Vector<ComplexNumber>(new ComplexNumber[] { num3, num4 });

            num4.DivideByZero += func;
            Vector<int> vc = new Vector<int>(new int[] { 0 });
            Console.WriteLine(num1.Module);
            Console.WriteLine(vec1 + vec2);




            //num4 = num4 / num5;
            /*
            List<Vector<ComplexNumber>> ls = new List<Vector<ComplexNumber>>();
            ls.Add(vec1);
            ls.Add(vec2);

            ls = Vector<ComplexNumber>.orthogonalize(ls);

            foreach(var val in ls)
            {
                  Console.WriteLine(val);
            }

            Console.WriteLine(ls[0] * ls[1]);
            */

        }
        static public void func(object obj, DivideByZeroEventArgs args)
        {
            Console.WriteLine("In func");
        }
    }
}

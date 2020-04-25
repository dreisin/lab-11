using System;
using System.Collections.Generic;
using System.Text;

namespace Task11
{
    class ComplexNumber : IEquatable<ComplexNumber>, IComparable<ComplexNumber>, IComparable
    {
        public double Re { get; private set; }//не всегда хороший вариант филд свойство 
        public double Im { get; private set; }// превращает в неизменяемый объект , свойство field, flyweight pattern

        public event EventHandler<DivideByZeroEventArgs> DivideByZero;

        public double Module { get { return Math.Sqrt(Re * Re + Im * Im); } }
        
        public double Argument { get { return Arg(); } }

        public ComplexNumber():this(0 , 0)
        { }

        public ComplexNumber(ComplexNumber val):this(val.Re , val.Im )
        { }

        public ComplexNumber(double x, double y)
        {
            Re = x;
            Im = y;
        }
        // 3 - ий контсруктор именованный
        // arg - в радианах
        public static ComplexNumber ComplexTrigonometricNumber(double arg, double mod)
        {
            return new ComplexNumber(mod * Math.Cos(arg), mod * Math.Sin(arg));
        }

        private double Arg()
        {
            double eps = 1e-10;
            if (Re > eps && Math.Abs(Im) < eps)
                return 0;
            if (Re > eps && Im > eps)
                return Math.Atan(Math.Abs(Im / Re));
            if (Math.Abs(Re) < eps && Im > eps)
                return Math.PI * 0.5;
            if (Re < -eps && Im > eps)
                return Math.PI - Math.Atan(Math.Abs(Im / Re));
            if (Re < -eps && Im < -eps)
                return Math.PI;
            if (Re < -eps && Im < -eps)
                return -Math.PI + Math.Atan(Math.Abs(Im / Re));
            if (Re < -eps && Im < -eps)
                return -Math.PI * 0.5;
            if (Re > eps && Im < -eps)
                return -Math.Atan(Math.Abs(Im / Re));

            return Math.Atan(Im / Re);
        }

        // сопряженное
        static public ComplexNumber Conjugate(ComplexNumber number)
        {
            return new ComplexNumber(number.Re, -number.Im);
        }

        static public ComplexNumber operator +(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return Sum(leftNum, rightNum);
        }

        static public ComplexNumber Sum(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return new ComplexNumber(leftNum.Re + rightNum.Re, leftNum.Im + rightNum.Im);
        }

        static public ComplexNumber operator -(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return Sub(leftNum, rightNum);
        }

        static public ComplexNumber Sub(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return new ComplexNumber(leftNum.Re - rightNum.Re, leftNum.Im - rightNum.Im);
        }

        //(x1 + iy1)(x2 + iy2) = x1x2 - y1y2 + i(x2y1 + y2x1)
        static public ComplexNumber operator *(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return Mul(leftNum, rightNum);
        }

        static public ComplexNumber Mul(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return new ComplexNumber(leftNum.Re * rightNum.Re - leftNum.Im * rightNum.Im,
                                    leftNum.Re * rightNum.Im + leftNum.Im * rightNum.Re);
        }

        static public ComplexNumber operator *(ComplexNumber complNum, double number)
        {
            return NumberMul(complNum, number);
        }

        static public ComplexNumber operator *(double number, ComplexNumber complNum)
        {
            return NumberMul(complNum, number);
        }

        static public ComplexNumber NumberMul(ComplexNumber complNum, double number)
        {
            return new ComplexNumber(complNum.Re * number,
                                   complNum.Im * number);
        }

        //(x1 + iy1) / (x2 + iy2) = (x1 + iy1) * (x2 - iy2) / (x2^2 + y2^2)
        static public ComplexNumber operator /(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            return Div(leftNum, rightNum);
        }

        static public ComplexNumber Div(ComplexNumber leftNum, ComplexNumber rightNum)
        {
            // Проверка на 0 с использованием события!
            double eps = 1e-10;

            if (rightNum.Module < eps)
            {
              
                DivideByZeroEventArgs args = new DivideByZeroEventArgs();//Можно сразу при помоши угловых в момент создания своего объекта после вызвова констуркоора сразу можно запихать ссылки
                args.Division = leftNum;
                args.Divider = rightNum;
                leftNum.DivideByZero?.Invoke(leftNum, args);

                throw new ComplexNumberException("Divide by zero");
            }

            ComplexNumber res = leftNum * ComplexNumber.Conjugate(rightNum);
            return new ComplexNumber(res.Re / (Math.Pow(rightNum.Re, 2) + Math.Pow(rightNum.Im, 2)),
                                        res.Im / (Math.Pow(rightNum.Re, 2) + Math.Pow(rightNum.Im, 2)));
        }

        static public ComplexNumber Pow(ComplexNumber num, int pow)
        {
            if (pow < 0)
                throw new ComplexNumberException("Not correct pow");

            if (pow == 0)
                return new ComplexNumber(1, 0);

            return ComplexNumber.ComplexTrigonometricNumber(pow * num.Argument, Math.Pow(num.Module, pow));
        }

        static public ComplexNumber[] Root(ComplexNumber num, int pow)
        {
            if (pow < 1)
                throw new ComplexNumberException("Not correct pow");

            if (pow == 1)
                return new ComplexNumber[]{ num };

            double arg = num.Argument;
            // извлекаем корень из модуля num
            double resMod = Math.Pow(num.Module, 1 / (double)pow);

            List<ComplexNumber> resList = new List<ComplexNumber>();

            for (int k = 0; k < pow; ++k)
            {
                resList.Add(ComplexNumber.ComplexTrigonometricNumber((arg + 2 * Math.PI * k) / pow, resMod));
            }

            return resList.ToArray();
        }
     
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if(obj is ComplexNumber)
            {
                return Equals(obj as ComplexNumber);
            }
            return false;
        }

        public bool Equals(ComplexNumber cnum)
        {
            if(cnum is null)
            {
                return false;
            }
            return Re.Equals(cnum.Re) && Im.Equals(cnum.Im);
        }
      

        public static implicit operator ComplexNumber(int num)
        {
            return new ComplexNumber(num, 0);
        }

        public override string ToString()
        {
            return $"[{Re} + {Im}i]";
        }

        
        

        public int CompareTo(object obj)
        {

            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            else if (obj is ComplexNumber)
            {
                return CompareTo(obj as ComplexNumber);
            }
            else
            {
                throw new ArgumentException("Invalid type", nameof(obj));
            }
          

         
        }
        public int CompareTo(ComplexNumber number)
        {

            if (number is null)
                throw new ArgumentNullException();

            if (!(number is ComplexNumber num))
                throw new ArgumentException("Argument is not Vector");

            if (Module > num.Module)
                return 1;
            if (Module < num.Module)
                return -1;
            if (num is null)
            {
                throw new ArgumentNullException("Cannot Compare");
            }
            if (num is ComplexNumber)
            {
                return Module.CompareTo(num.Module);
            }
            return 0;

        }

            //CompareTo за тем чтобы если сравнивать объект с чем угодно аналогия с equals
        public static bool operator >(ComplexNumber a, ComplexNumber b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(ComplexNumber a, ComplexNumber b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(ComplexNumber a, ComplexNumber b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(ComplexNumber a, ComplexNumber b)
        {
            return a.CompareTo(b) >= 0;
        }
    }
}

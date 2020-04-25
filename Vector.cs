using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Task11
{
    class Vector<T> : IEnumerable<T>, IComparable<Vector<T>> where T: IEquatable<T>, IComparable<T>, new()
    {
        private List<T> _vector;
        public int Size { get { return _vector.Count; } }

        public T Module { get { return Mod(); } }

        public Vector()
        {
            _vector = new List<T>();
        }

        public Vector(IEnumerable<T> vec) : this()
        {
            foreach(var val in vec)
            {
                _vector.Add(val);
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                    throw new VectorException("Not correct index");

                return _vector[index];
            }

            set
            {
                if (index < 0 || index >= Size)
                    throw new VectorException("Not correct index");

                _vector[index] = value;
            }
        }

        public void Add(T val)
        {
            _vector.Add(val);
        }

        static public Vector<T> operator +(Vector<T> leftVector, Vector<T> rightVector)
        {
            return Sum(leftVector, rightVector);
        }

        static public Vector<T> Sum(Vector<T> leftVector, Vector<T> rightVector)
        {
            if (leftVector.Size != rightVector.Size)
                throw new VectorException("Not equals size");

            Vector<T> res = new Vector<T>();

            for (int i = 0; i < leftVector.Size; ++i)
            {
                res.Add((dynamic)leftVector[i] + rightVector[i]);
            }

            return res;
        }

        static public Vector<T> operator -(Vector<T> leftVector, Vector<T> rightVector)
        {
            return Sub(leftVector, rightVector);
        }

        static public Vector<T> Sub(Vector<T> leftVector, Vector<T> rightVector)
        {
            if (leftVector.Size != rightVector.Size)
                throw new VectorException("Not equals size");

            Vector<T> res = new Vector<T>();

            for (int i = 0; i < leftVector.Size; ++i)
            {
                res.Add((dynamic)leftVector[i] - rightVector[i]);
            }

            return res;
        }

        static public Vector<T> operator *(Vector<T> vector, T number)
        {
            return NumberMul(vector, number);
        }

        static public Vector<T> operator *(T number, Vector<T> vector)
        {
            return NumberMul(vector, number);
        }

        static public Vector<T> NumberMul(Vector<T> vector, T number)
        {
            Vector<T> res = new Vector<T>();

            for (int i = 0; i < vector.Size; ++i)
            {
                res.Add((dynamic)vector[i] * number);
            }

            return res;
        }

        // скалярное произведение
        static public T operator *(Vector<T> leftVector, Vector<T> rightVector)
        {
            return ScalarMul(leftVector, rightVector);
        }

        static public T ScalarMul(Vector<T> leftVector, Vector<T> rightVector)
        {
            if (leftVector.Size != rightVector.Size)
                throw new VectorException("Not equals size");

            T res = new T();
            if (res is ComplexNumber)
            {
                for (int i = 0; i < leftVector.Size; ++i)
                {
                    res = res + leftVector[i] * ComplexNumber.Conjugate((dynamic)rightVector[i]);
                }
            }
            else
            {
                for (int i = 0; i < leftVector.Size; ++i)
                {
                    res = res + (dynamic)leftVector[i] * rightVector[i];
                }
            }
            return res;
        }

        // Корень квадратный из суммы квадратов модулей TODO
        public T Mod()
        {
            T res = new T();
            double result = 0.0;
            if (res is ComplexNumber)
            {
                // возвращать массив 
                foreach (var val in _vector)
                {
                    result += Math.Pow(((ComplexNumber)(dynamic)val).Module, 2);
                    //return ComplexNumber.Sqrt((dynamic)this * this, 2)[0];
                    //скалярное произведение это комплескнкое число, оке, тогда раз мы зом взять корень из комплескноного числа тогда мы получаем массив комплесным чисел а корней у комплексного числа несколько и остальные числа мы отбрасываем
                    // Нам ну
                }
                return  (T)(dynamic)(new ComplexNumber (Math.Sqrt(result), 0));
            }
            else
            {
                return (T)Math.Sqrt((dynamic)this * this);//Сложить все элементы веткора и затем корень
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _vector.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if(obj is Vector<T>)
            {
                return Equals((Vector<T>)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Vector<T> vec)
        {
            //Null
            if (Size != vec.Size)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Size; ++i)
                {
                    if (!this[i].Equals(vec[i]))
                        return false;
                }
                return true;
            }
        }

        // ортогонализация по грамму-шмидту
        static public List<Vector<T>> orthogonalize(IEnumerable<Vector<T>> collections)
        {
            List<Vector<T>> resList = new List<Vector<T>>();

            foreach(var f_n in collections) // f_n - вектора, которые надо ортогонализовать
            {
                Vector<T> res = f_n;
                foreach(var g_n in resList) // g_n - уже ортогонализованные вектора
                {
                    res = res - ((dynamic)f_n * g_n) / (g_n * g_n) * g_n;
                }
                resList.Add(res);
            }

            return resList;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder("[");

            foreach(var val in _vector)
            {
                str.Append(string.Format("{0} ", val));
            }

            return str.Remove(str.Length - 1, 1).Append("]").ToString();
        }

        public T[] ToArray()
        {
            return _vector.ToArray();
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            else if (obj is Vector<T>)
            {
                return CompareTo(obj as Vector<T>);
            }
            else
            {
                throw new ArgumentException("Invalid type", nameof(obj));
            }
        }

        public int CompareTo(Vector<T> vec)
        {
            if (vec is null)
            {
                throw new ArgumentNullException(nameof(vec));
            }
            if (_vector.Count != vec._vector.Count)
            {
                throw new ArithmeticException("Invalid sizes");
            }

            return this.Module.CompareTo(vec.Module);
        }

        //public int CompareTo(object obj)
        //{
        //    if (obj is null)
        //        throw new ArgumentNullException();

        //    if (!(obj is Vector<T> vec))
        //        throw new ArgumentException("Argument is not Vector");

        //    if ((dynamic)Mod() > (dynamic)vec.Mod())
        //        return 1;
        //    if ((dynamic)Mod() < (dynamic)vec.Mod())
        //        return -1;

        //    return 0;
        //}

        //public int CompareTo(Vector<T> vec)
        //{
        //    if(vec is null)
        //    {
        //        throw new ArgumentException("Not Correct Value!");
        //    }
        //    return this.Module.CompareTo(vec.Module);
        //}

        public static bool operator >(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) >= 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Classes
{
    public class ER_Ref<T>
    {
        public ER_Ref() { }
        public ER_Ref(T error) { Error = error; }
        public T Error { get; set; }
        public override string ToString()
        {
            T error = Error;
            return error == null ? "" : error.ToString();
        }
        public static implicit operator T(ER_Ref<T> r) { return r.Error; }
        public static implicit operator ER_Ref<T>(T error) { return new ER_Ref<T>(error); }
    }

    public class Ref<T>
    {
        public Ref() { }
        public Ref(T value) { Value = value; }
        public T Value { get; set; }
        public override string ToString()
        {
            T value = Value;
            return value == null ? "" : value.ToString();
        }
        public static implicit operator T(Ref<T> r) { return r.Value; }
        public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }
    }
    public class RowT : Dictionary<string, string>
    {
    }
    public class PaginationList<T> : List<T>
    {
        public PaginationList():base(){}

        public PaginationList(List<T> ts)
        {
            this.AddRange(ts);
        }
        public int ItemsCount { get; set; } = 0;
    }
}

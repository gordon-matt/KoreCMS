using System.Collections.Generic;

namespace Kore.Collections.Generic
{
    public class TripleList<TFirst, TSecond, TThird> : List<Triple<TFirst, TSecond, TThird>>
    {
        public void Add(TFirst first, TSecond second, TThird third)
        {
            this.Add(new Triple<TFirst, TSecond, TThird>
            {
                First = first,
                Second = second,
                Third = third
            });
        }
    }
}
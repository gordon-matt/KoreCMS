using System.Collections.Generic;

namespace Kore.Collections.Generic
{
    public class PairList<TFirst, TSecond> : List<Pair<TFirst, TSecond>>
    {
        public void Add(TFirst first, TSecond second)
        {
            Add(new Pair<TFirst, TSecond>
            {
                First = first,
                Second = second
            });
        }
    }
}
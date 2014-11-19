﻿namespace Kore
{
    public interface ICloneable<T>
    {
        T ShallowCopy();

        T DeepCopy();
    }
}
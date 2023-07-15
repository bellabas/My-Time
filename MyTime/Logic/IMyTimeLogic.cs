using System;

namespace MyTime.Logic
{
    public interface IMyTimeLogic
    {
        TimeSpan MyElapsedTime { get; }

        void Calculate(DateTime dob);
    }
}
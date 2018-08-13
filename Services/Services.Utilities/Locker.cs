using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Utilities
{
    public static class Locker
    {
        public static T LockedResult<T>(this object obj, Func<bool> criteria, Func<T> locked, Func<T> unlocked)
        {
            if (criteria())
                lock (obj)
                {   
                    if (criteria())
                        return locked();
                }

            return unlocked();
        }


        public static void LockedAction(this object obj, Func<bool> test, Action passedAction)
        {
            if (test())
                lock (obj)
                {
                    if (test())
                    {
                        passedAction();
                        return;
                    }
                }
        }



    }
}

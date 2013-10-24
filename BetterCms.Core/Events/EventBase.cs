using System;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public delegate void DefaultEventHandler<in TArgs>(TArgs args) where TArgs : EventArgs;

    public abstract class EventsBase<TEvents> where TEvents : class, new()
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly object lockObject = new object();
// ReSharper restore StaticFieldInGenericType
        
        private static volatile TEvents instance;        

        public static TEvents Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new TEvents();
                        }
                    }
                }

                return instance;
            }
        }
    }
}

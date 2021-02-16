public class Singleton<T> where T : class, new()
{
    private static T instance = default(T);
    private static readonly object locker = new object();
    public static T Inst
    {
        get
        {
            lock (locker)
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
        }
    }

    public virtual void Init(){}
}


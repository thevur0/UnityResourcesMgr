

public class Singleton<T> where T: class,new()
{
    static T ms_Instance = null;
    static public T Instance
    {
        get
        {
            if (ms_Instance == null)
                ms_Instance = new T();
            return ms_Instance;
        }
    }
    public static T GetInstance()
    {
        return Instance;
    }
}

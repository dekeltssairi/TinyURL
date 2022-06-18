namespace TinyUrl
{
    public static class Locker
    {
        public static readonly object _locker = new object();
    }
}

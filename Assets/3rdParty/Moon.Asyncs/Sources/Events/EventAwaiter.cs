namespace Moon.Asyncs
{
    public class EventAwaiter
    {
        public AsyncAwaiter State = new AsyncAwaiter();

        public void Invoke()
        {
            if (State == null)
            {
                return;
            }
            State.Complete();
            State = null;
        }
    }
}

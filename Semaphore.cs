using System.Threading;

namespace AirportSim
{
    public class Semaphore
    {
        public int Count { get; private set; }
        public bool Waiting { get; private set; }
        public void Wait()
        {
            lock (this)
            {
                while (Count == 0)
                {
                    Waiting = true;
                    Monitor.Wait(this);
                }
                Count = 0;
                Waiting = false;
            }
        }
        public void Signal()
        {
            lock (this)
            {
                Count = 1;
                Monitor.Pulse(this);
            }
        }
        public void Start()
        {
            
        }
    }
}
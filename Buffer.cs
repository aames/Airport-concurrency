using System.Threading;

namespace AirportSim
{
    public class Buffer
    {
        private Plane _plane;
        private bool _empty = true;
        public void Read(ref Plane plane)
        {
            lock (this)
            {
                if (_empty)
                    Monitor.Wait(this);
                _empty = true;
                plane = _plane;
                Monitor.Pulse(this);
            }
        }
        public void Write(Plane plane)
        {
            lock(this)
            {
                if (!_empty)
                    Monitor.Wait(this);
                _empty = false;
                _plane = plane;
                Monitor.Pulse(this);
            }
        }
        public void Start()
        {
            
        }
    }
}
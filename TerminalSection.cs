using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AirportSim
{
    public class TerminalSection : Section
    {
        public Semaphore SemaphoreIn { get; set; }
        public Semaphore SemaphoreOut { get; set; }
        public Buffer BufferIn { get; set; }
        public Buffer BufferOut { get; set; }

        public bool Hold { get; set; }

        public TerminalSection(Panel panel) : base(panel)
        {
            panel.Paint += Panel_Paint;
        }
        void Panel_Paint(object sender, PaintEventArgs e)
        {
            if (Plane == null)
                e.Graphics.Clear(Color.White);
            else
                Plane.Display(e.Graphics);
        }
        public void Run()
        {
            while (true)
            {
                var plane = Plane;
                SemaphoreIn.Signal();
                BufferIn.Read(ref plane);
                Plane = plane;
                if (Plane == null)
                {
                    continue;
                }
                Plane = plane;
                Plane.Angle = Angle;
                Reset();
                while (!AtEnd())
                {
                    plane.Advance();
                    Panel.Invalidate();
                    Thread.Sleep(20);
                }
                while (Hold)
                {}
                plane.Destination = 0;
                ReverseDirection();
                Plane.Angle = Angle;
                while (!AtEnd())
                {
                    plane.Advance();
                    Panel.Invalidate();
                    Thread.Sleep(20);
                }
                SemaphoreOut.Wait();
                BufferOut.Write(plane);
                Plane = null;
                Panel.Invalidate();
                ReverseDirection();
            }
        }
        public override bool AtEnd()
        {
            var rtn = false;
            if (Angle == 0)
                rtn = Plane.Position.Y <= 25;
            else if (Angle == 90)
                rtn = Plane.Position.X >= Panel.Size.Width - 25;
            else if (Angle == 180)
                rtn = Plane.Position.Y >= Panel.Size.Height - 25;
            else if (Angle == 270)
                rtn = Plane.Position.X >= 25;
            return rtn;
        }

    }
}
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace AirportSim
{
    public class TaxiSection : Section
    {
        public Buffer Buffer1In { get; set; } //there is only 1 in buffer
        public Buffer Buffer1OutToTerm { get; set; }
        public Buffer Buffer2OutToTaxi { get; set; }
        public Semaphore Semaphore1In { get; set; }
        public Semaphore Semaphore1OutToTerm { get; set; }
        public Semaphore Semaphore2OutToTaxi { get; set; }
        public int Buffer1OutDestination { get; set; }


        public TaxiSection(Panel panel) : base(panel)
        {
            panel.Paint+= new PaintEventHandler(Panel_Paint);
        }
        void Panel_Paint(object sender, PaintEventArgs e)
        {
            if(Plane == null)
                e.Graphics.Clear(Color.White);
            else
                Plane.Display(e.Graphics);
        }
        public void Run()
        {
            while (true)
            {
                var plane = Plane;
                Semaphore1In.Signal();
                Buffer1In.Read(ref plane);
                Plane = plane;
                if(Plane == null)
                {
                    continue;
                }
                Plane = plane;
                Plane.Angle = this.Angle;
                Reset();
                while (!AtEnd())
                {
                    Plane.Advance();
                    Panel.Invalidate();
                    Thread.Sleep(20);
                }
                if(Plane.Destination == Buffer1OutDestination)
                {
                    Semaphore1OutToTerm.Wait();
                    Buffer1OutToTerm.Write(plane);
                }
                else
                {
                    Semaphore2OutToTaxi.Wait();
                    Buffer2OutToTaxi.Write(plane);
                }
                Plane = null;
                Panel.Invalidate();
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
                rtn = Plane.Position.X <= 25; 
            return rtn;
        }
    }
}
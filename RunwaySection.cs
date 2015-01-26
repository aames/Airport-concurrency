using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AirportSim
{
    public class RunwaySection : Section
    {
        public Buffer Buffer1InLand { get; set; }
        public Buffer Buffer2In { get; set; }
        public Buffer Buffer3Out { get; set; }
        public Semaphore Semaphore1InLand{ get; set; }
        public Semaphore Semaphore2In{ get; set; }
        public Semaphore Semaphore3Out{ get; set; }
        public Button Button{ get; set; }
        public Label TotalLanded { get; set; }
        public Label TotalTakeOffs { get; set; }
        public bool Land { get; set; }

        public RunwaySection(Panel panel) : base(panel)
        {
            Panel.Paint += new PaintEventHandler(Panel_Paint);
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
                if (Semaphore2In.Waiting)
                {
                    Semaphore2In.Signal();
                    Buffer2In.Read(ref plane);
                }
                else if (Semaphore1InLand.Waiting && Land)
                {
                    Semaphore1InLand.Signal();
                    Buffer1InLand.Read(ref plane);
                    TotalLanded.BeginInvoke(new MethodInvoker(UpdateLandedCount));

                }
                else
                {
                    continue;
                }
                Plane = plane;
                Plane.Angle = Angle;
                Reset();

                while(!AtEnd())
                {
                    if (Plane.Destination != 0)
                    {
                        if (Plane.Speed > 2)
                            Plane.Speed -= 1;
                    }
                    else
                        if (Plane.Speed < 20)
                            Plane.Speed += 1;

                    plane.Advance();
                    Panel.Invalidate();
                    Thread.Sleep(20);
                }
                if (plane.Destination == 0)
                {
                    Plane = null;
                    Panel.Invalidate();
                    TotalTakeOffs.BeginInvoke(new MethodInvoker(UpdateTakeOffCount));
                    continue;
                }
                Semaphore3Out.Wait();
                Buffer3Out.Write(Plane);
                Plane = null; 
                Panel.Invalidate();
            }
        }
        public override bool AtEnd()
        {
            bool rtn;
            if (Plane.Destination == 0)
            {
                rtn = Plane.Position.X <= 25;
            }
            else
            {
                rtn = Plane.Position.X <= 25 + 95;
            }
            return rtn;
        }
        public void UpdateLandedCount()
        {
            var count = int.Parse(TotalLanded.Text);
            TotalLanded.Text = ""+ (count + 1);
            
        }
        public void UpdateTakeOffCount()
        {
            var count = int.Parse(TotalTakeOffs.Text);
            TotalTakeOffs.Text = "" + (count + 1);
        }
    }
}
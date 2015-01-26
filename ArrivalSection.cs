using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AirportSim
{
    public class ArrivalSection
    {
        public Queue<Plane> Planes { get; set; }
        public Panel Panel { get; set; }
        public int Sleep { get; set; }
        public Buffer BufferOut { get; set; }
        public Semaphore SemaphoreOut { get; set; }
        public bool Land { get; set; }
        public RadioButton RadioBtn1 { get; set; }
        public RadioButton RadioBtn2 { get; set; }
        public RadioButton RadioBtn3 { get; set; }
        public RadioButton RadioBtn4 { get; set; }

        public ArrivalSection(Panel panel)
        {
            Panel = panel;
            Planes = new Queue<Plane>();
            Panel.Paint += new PaintEventHandler(Panel_Paint);

        }

        void Panel_Paint(object sender, PaintEventArgs e)
        {
            int y = Panel.Size.Height-25;

            foreach (var plane in Planes.ToArray())
            {
                plane.Angle = 180;
                plane.Position = new Point(25, y);
                y = y - 50;
                plane.Display(e.Graphics);
            }
        }
        public void Run ()
        {
            while (true)
            {
                if (!Land)
                    continue;
                if (Planes.Count==0)
                    continue;
                SemaphoreOut.Wait();
                var plane = Planes.Dequeue();
                if (RadioBtn4.Checked)
                {
                    plane.Destination = 0;
                    plane.Speed = 5;
                }
                else if (RadioBtn1.Checked)
                {
                    plane.Destination = 1;
                }
                else if (RadioBtn2.Checked)
                {
                    plane.Destination = 2;
                }
                else if (RadioBtn3.Checked)
                {
                    plane.Destination = 3;
                }
                if (Land)
                    BufferOut.Write(plane);
                Panel.Invalidate();

            }
        }
        public void AddPlaneToQueue()
        {
            while (true)
            {
                Thread.Sleep(1000*Sleep);
                if (Planes.Count>=8)
                {
                    continue;
                }
                var plane = new Plane() {Speed = 30};
                Planes.Enqueue(plane);
                Panel.Invalidate();
            }
        }
    }
}

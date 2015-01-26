using System.Drawing;
using System.Windows.Forms;

namespace AirportSim
{
    public class Section
    {
        public Panel Panel { get; set; }
        public Plane Plane { get; set; }
        public int Angle { get; set; }
        
        public Section(Panel panel)
        {
            Panel = panel;
        }
        public virtual bool AtEnd()
        {
            return false;
        }
        public virtual void Reset()
        {
            switch (Plane.Angle)
            {
                case 0:
                    Plane.Position = new Point(Panel.Width/2, Panel.Height);
                    break;
                case 90:
                    Plane.Position = new Point(Panel.Height/2,25);
                    break;
                case 180:
                    Plane.Position = new Point(Panel.Width/2, 25);
                    break;
                case 270:
                    Plane.Position = new Point(Panel.Width - 25, Panel.Height / 2); //these where back wards s
                    break;
            }
        }
        public void ReverseDirection()
        {
            switch (Angle)
            {
                case 0:
                    Angle = 180;
                    break;
                case 90:
                    Angle = 270;
                    break;
                case 180:
                    Angle = 0;
                    break;
                case 270:
                    Angle = 90;
                    break;
            }
        }
    }
}

using System;
using System.Drawing;

namespace AirportSim
{
    public class Plane
    {
        public Color Color { get; private set; }
        public Point Position { get; set; }
        public int Angle { get; set; }
        public int Speed { get; set; }
        public int Destination { get; set; }

        public Plane()
        {
            var r = new Random();
            Color = Color.FromArgb(r.Next(1,255), r.Next(50,255), r.Next(1,255));
            Speed = 2;
        }

        public void Display(Graphics c)
        {
            var matrix = new[,] {{0, -20}, {-20, 20}, {20, 20}};
            var brush = new SolidBrush(Color);
            var theta = Angle*(Math.PI/180);
            var sine = Math.Sin(theta);
            var cosine = Math.Cos(theta);
            var point = new Point[3];
            for (int i = 0; i < 3; i++)
            {
                var x = matrix[i, 0];
                var y = matrix[i, 1];
                var newX = (x*cosine) - (y*sine);
                var newY = (x*sine) + (y*cosine);
                point[i] = new Point((int) newX + Position.X, (int) newY + Position.Y);
            }
            c.FillPolygon(brush, point);
            c.DrawString(Destination.ToString(),new Font(FontFamily.GenericSerif, 12f,FontStyle.Bold), new SolidBrush(Color.White),Position.X - 5,Position.Y- 7);
            brush.Dispose();
        }
        public void Advance ()
        {
            switch (Angle)
            {
                case 0: 
                    Position = new Point(Position.X, Position.Y-Speed); break;
                case 90: 
                    Position = new Point(Position.X+Speed, Position.Y); break;
                case 180:
                    Position = new Point(Position.X, Position.Y + Speed); break;
                case 270:
                    Position = new Point(Position.X-Speed, Position.Y); break;
            }
        }
    }
}

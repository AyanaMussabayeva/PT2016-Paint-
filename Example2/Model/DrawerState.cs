using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example2.Model
{
    public enum Shape
    {
        Line,
        Circle,
        Rectangle,
        Triangle,
        Pencil,
        Eraser
    }
    public enum DrawTool
    {
        Pen,
        Brus

    }

    public class DrawerState
    {
        public Pen pen = new Pen(Color.Red);
        Rectangle r;
        public Bitmap bmp;
        Graphics g;
        GraphicsPath path;
        private PictureBox pictureBox1;

        public Point prevPoint;

        public DrawTool DrawTool { get; set; }
        public Shape Shape { get; set; }

        public void FixPath()
        {
            if (path != null)
            {
                g.DrawPath(pen, path);
                path = null;
            }
        }

        public DrawerState(PictureBox pictureBox1)
        {
            this.pictureBox1 = pictureBox1;
            
            Load("");

            DrawTool = DrawTool.Pen;
            Shape = Shape.Pencil;

            pictureBox1.Paint += PictureBox1_Paint;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (path != null)
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        public void Draw(Point currentPoint, int x, int y)
        {
            int width, height;
            switch (Shape)
            {
                case Shape.Line:
                    path = new GraphicsPath();
                    path.AddLine(prevPoint, currentPoint);
                    break;
                case Shape.Circle:
                    path = new GraphicsPath();
                    path.AddEllipse(prevPoint.X, prevPoint.Y, currentPoint.X - prevPoint.X, currentPoint.Y - prevPoint.Y);
                    /*width = Math.Abs(currentPoint.X - prevPoint.X);
                    height = Math.Abs(currentPoint.Y - prevPoint.Y);
                    if (x > prevPoint.X && y > prevPoint.Y)
                        r = new Rectangle(prevPoint.X, prevPoint.Y, width, height);
                    else if (prevPoint.X > x && prevPoint.Y > y)
                        r = new Rectangle(currentPoint.X, currentPoint.Y, width, height);
                    else if (prevPoint.X > x && y > prevPoint.Y)
                        r = new Rectangle(currentPoint.X, prevPoint.Y, width, height);
                    else if (prevPoint.Y > y && x > prevPoint.X)
                        r = new Rectangle(prevPoint.X, currentPoint.Y, width, height);
                    path.AddEllipse(r);*/

                    break;
                case Shape.Rectangle:
                    path = new GraphicsPath();
                    width = Math.Abs(currentPoint.X - prevPoint.X);
                    height = Math.Abs(currentPoint.Y - prevPoint.Y);
                    if (x > prevPoint.X && y > prevPoint.Y)
                        r = new Rectangle(prevPoint.X, prevPoint.Y, width, height);
                    else if (prevPoint.X > x && prevPoint.Y > y)
                        r = new Rectangle(currentPoint.X, currentPoint.Y, width, height);
                    else if (prevPoint.X > x && y > prevPoint.Y)
                        r = new Rectangle(currentPoint.X, prevPoint.Y, width, height);
                    else if (prevPoint.Y > y && x > prevPoint.X)
                        r = new Rectangle(prevPoint.X, currentPoint.Y, width, height);
                    path.AddRectangle(r);
                    break;
                case Shape.Triangle:
                    Point trianglePoint1 = new Point(((currentPoint.X - prevPoint.X)/2 + prevPoint.X), prevPoint.Y);
                    Point trianglePoint2 = new Point(currentPoint.X, currentPoint.Y);
                    Point trianglePoint3 = new Point(prevPoint.X, currentPoint.Y);
                    PointF[] triangleP = { trianglePoint1, trianglePoint2, trianglePoint3 };
                    path = new GraphicsPath();
                    path.AddPolygon(triangleP);
                    break;
                case Shape.Pencil:
                    g.DrawLine(pen, prevPoint, currentPoint);
                    prevPoint = currentPoint;
                    break;
                case Shape.Eraser:
                    g.DrawLine(new Pen(Color.White,pen.Width), prevPoint, currentPoint);
                    prevPoint = currentPoint;
                    break;
                default:
                    break;
            }

            pictureBox1.Refresh();
        }

        public void Save(string fileName)
        {
            bmp.Save(fileName);
        }

        public void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                bmp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            }
            else {
                bmp = new Bitmap(fileName);
            }

            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
        }
    }
}

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;

namespace CadDemo.OverruleLearn
{
    internal class LineToPipe : DrawableOverrule
    {
        private double r;
        public LineToPipe(double r)
        {
            this.r = r;
        }

        public override bool WorldDraw(Drawable drawable, WorldDraw wd)
        {
            if(drawable is Line)
            {
                Line line = (Line) drawable;
                Circle circle = new Circle(line.StartPoint,line.EndPoint-line.StartPoint,r);
                ExtrudedSurface pipe = new ExtrudedSurface();
                pipe.CreateExtrudedSurface(circle, line.EndPoint - line.StartPoint, new SweepOptions());
                pipe.WorldDraw(wd);
                circle.Dispose();
                pipe.Dispose();
            }
            return true;
        }
    }
}
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;

namespace CadDemo.JigLearn
{
    public class PolyLineJig : EntityJig
    {
        const double kBulge = 1.0;
        Point3d tempPoint;
        Plane plane;

        private bool isArcSeg = false;
        public bool IsArcSeg
        {
            get { return isArcSeg; }
        }
        private bool isUndoing = false;
        public bool IsUndoing
        {
            get { return isUndoing; }
        }

        public PolyLineJig(Matrix3d ucs) : base(new Polyline())
        {
            Point3d origin = new Point3d(0, 0, 0);
            Vector3d normal = new Vector3d(0, 0, 1);
            normal = normal.TransformBy(ucs);
            plane = new Plane(origin, normal);

            Polyline polyline = Entity as Polyline;
            polyline.SetDatabaseDefaults();
            polyline.Normal = normal;
            AddDummyVertex();
        }

        public void AddDummyVertex()
        {
            Polyline polyline = Entity as Polyline;
            polyline.AddVertexAt(polyline.NumberOfVertices, new Point2d(0, 0), 0, 0, 0);
        }

        public void RemoveDummyVertex()
        {
            Polyline polyline = Entity as Polyline;
            if(polyline.NumberOfVertices >0)
            {
                polyline.RemoveVertexAt(polyline.NumberOfVertices - 1);
            }
            if(polyline.NumberOfVertices >= 2)
            {
                double bulge = polyline.GetBulgeAt(polyline.NumberOfVertices - 2);
                isArcSeg = (bulge != 0);
            }
        }

        public void AdjustSegmentType(bool isArc)
        {
            double bulge = 0;
            if(isArc)
            {
                bulge = kBulge;
            }
            Polyline polyline = Entity as Polyline;
            if(polyline.NumberOfVertices >= 2)
            {
                polyline.SetBulgeAt(polyline.NumberOfVertices-2, bulge);
            }
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions jigPromptPointOptions = new JigPromptPointOptions();
            jigPromptPointOptions.UserInputControls = (UserInputControls.Accept3dCoordinates | UserInputControls.NoZeroResponseAccepted |
                UserInputControls.NoNegativeResponseAccepted);
            isUndoing = false;

            Polyline polyline = Entity as Polyline;
            if(polyline.NumberOfVertices == 1)
            {
                jigPromptPointOptions.Message = "\n选择多段线起点：";
            }
            else if(polyline.NumberOfVertices > 1)
            {
                if(isArcSeg)
                {
                    jigPromptPointOptions.SetMessageAndKeywords("\n选择圆弧终点[Line/Undo]：", "Line Undo");
                }
                else
                {
                    jigPromptPointOptions.SetMessageAndKeywords("\n选择下一顶点[Arc/Undo]：", "Arc Undo");
                }
            }
            else
            {
                return SamplerStatus.Cancel;
            }

            PromptPointResult promptPointResult = prompts.AcquirePoint(jigPromptPointOptions);
            if(promptPointResult.Status == PromptStatus.Keyword)
            {
                if(promptPointResult.StringResult == "Arc")
                {
                    isArcSeg = true;
                }
                else if(promptPointResult.StringResult == "Line")
                {
                    isArcSeg = false;
                }
                else if( promptPointResult.StringResult == "Undo")
                {
                    isUndoing= true;
                }
                return SamplerStatus.OK;
            }
            else if(promptPointResult.Status == PromptStatus.OK)
            {
                if(tempPoint == promptPointResult.Value)
                {
                    return SamplerStatus.NoChange;
                }
                else
                {
                    tempPoint = promptPointResult.Value;
                    return SamplerStatus.OK;
                }
            }
            return SamplerStatus.Cancel;
        }

        protected override bool Update()
        {
            Polyline polyline = Entity as Polyline;
            polyline.SetPointAt(polyline.NumberOfVertices - 1, tempPoint.Convert2d(plane));
            if (isArcSeg)
            {
                polyline.SetBulgeAt(polyline.NumberOfVertices - 1, kBulge);
            }
            else
            {
                polyline.SetBulgeAt(polyline.NumberOfVertices - 1, 0);
            }
            return true;
        }

        public Entity GetEntity()
        {
            return Entity;
        }
    }
}

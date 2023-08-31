using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.EntityLearn.Learning2_1))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_1
    {
        /// <summary>
        /// 创建简单实体
        /// </summary>
        [CommandMethod("AddSimpleEntity")]
        public void AddSimpleEntity()
        {
            DBText dBText = CreateDBText(new Point3d(), "bittersweet.love", 10);
            ToModelSpace(dBText);
            Point3dCollection point3DCollection = new Point3dCollection()
            {
                new Point3d(20,10,0),
                new Point3d(35,-5,0),
                new Point3d(60,5,0),
                new Point3d(80,0,0)
            };
            Polyline3d polyline3D = new Polyline3d(Poly3dType.QuadSplinePoly, point3DCollection,true);
            ToModelSpace(polyline3D);
            Circle circle = new Circle(Point3d.Origin,Vector3d.ZAxis,15);
            RadialDimension radialDimension = CreateRadialDimension(circle, 3 * Math.PI / 4, 10);
            ToModelSpace(circle);
            ToModelSpace(radialDimension);
            OrdinateDimension ordinateDimension = CreateOrdinateDimensionX(circle.Center,new Point3d(10,10,0),10,true);
            ToModelSpace(ordinateDimension);
        }

        /// <summary>
        /// 由标注起点，长度方向创建X坐标标注
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="leadLength"></param>
        /// <param name="isBelow"></param>
        /// <returns></returns>
        private OrdinateDimension CreateOrdinateDimensionX(Point3d startPoint, Point3d ordPoint,double leadLength,bool isBelow)
        {
            Database database = HostApplicationServices.WorkingDatabase;
            Point3d endPoint = new Point3d(ordPoint.X + leadLength * Math.Cos(isBelow ? 3 * Math.PI / 2 : Math.PI / 2), ordPoint.Y + leadLength * Math.Cos(isBelow ? 3 * Math.PI / 2 : Math.PI / 2), ordPoint.Z);
            OrdinateDimension dimension = new OrdinateDimension(true, ordPoint, endPoint, "<我的坐标标注>", database.Dimstyle);
            dimension.Origin = startPoint;
            return dimension;
        }


        /// <summary>
        /// 由圆，角度，引线长度创建半径标注
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="angle"></param>
        /// <param name="leadLength"></param>
        /// <returns></returns>
        private RadialDimension CreateRadialDimension(Circle circle,double angle,double leadLength)
        {
            Point3d centerPoint = new Point3d(circle.Center.X,circle.Center.Y,0);
            Point3d chordPoint = new Point3d(centerPoint.X + Math.Cos(angle) * circle.Radius,centerPoint.Y + Math.Sin(angle) * circle.Radius,centerPoint.Z);
            Database database = HostApplicationServices.WorkingDatabase;
            RadialDimension dimension = new RadialDimension(centerPoint, chordPoint,leadLength,"<haha>",database.Dimstyle);
            return dimension;
        }

        /// <summary>
        /// 由插入点，文字内容，文字高度创建单行文字
        /// </summary>
        /// <param name="position">基点</param>
        /// <param name="textString">文字内容</param>
        /// <param name="textHeight">文字高度</param>
        /// <returns>单行文字</returns>
        private DBText CreateDBText(Point3d position,string textString,double textHeight)
        {
            DBText text = new DBText(); 
            text.Position = position;
            text.TextString = textString;
            text.Height = textHeight;
            return text;
        }

        /// <summary>
        /// 添加对象到模型空间
        /// </summary>
        /// <param name="entity">要添加的对象</param>
        /// <returns>对象ObjectId</returns>
        private ObjectId ToModelSpace(Entity entity)
        {
            Database database = HostApplicationServices.WorkingDatabase;
            ObjectId objectId;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                objectId = modelSpace.AppendEntity(entity);
                transaction.AddNewlyCreatedDBObject(entity,true);
                transaction.Commit();
            }
            return objectId;

        }
    }
}

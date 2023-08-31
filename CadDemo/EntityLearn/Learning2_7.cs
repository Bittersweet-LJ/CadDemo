using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.EntityLearn.Learning2_7))]
namespace CadDemo.EntityLearn
{
    //注释比例
    internal class Learning2_7
    {
        /// <summary>
        /// 添加注释比例的文字大小     会随绘图比例变化
        /// 没有添加注释比例的Circle大小   不会随绘图比例变化
        /// </summary>
        [CommandMethod("AutoScale")]
        public void AutoScale()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            DBText text = CreateDBText(Point3d.Origin, "AutoScale", 100);
            text.Annotative = AnnotativeStates.True;
            ObjectContextManager objectContextManager = database.ObjectContextManager;
            ObjectContextCollection objectContextCollection = objectContextManager.GetContextCollection("ACDB_ANNOTATIONSCALES");
            foreach (var objectContext in objectContextCollection)
            {
                text.AddContext(objectContext);
            }

            Circle circle = new Circle(new Point3d(265,50,0),Vector3d.ZAxis,300);
            ToModelSpace(text);
            ToModelSpace(circle);
        }


        /// <summary>
        /// 由插入点，文字内容，文字高度创建单行文字
        /// </summary>
        /// <param name="position">基点</param>
        /// <param name="textString">文字内容</param>
        /// <param name="textHeight">文字高度</param>
        /// <returns>单行文字</returns>
        private DBText CreateDBText(Point3d position, string textString, double textHeight)
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
                transaction.AddNewlyCreatedDBObject(entity, true);
                transaction.Commit();
            }
            return objectId;

        }
    }


}

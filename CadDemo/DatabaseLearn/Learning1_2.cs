using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.DatabaseLearn.Learning1_2))]
namespace CadDemo.DatabaseLearn
{
    internal class Learning1_2
    {
        /// <summary>
        /// 添加实体到模型空间
        /// </summary>
        [CommandMethod("AddEntToModelSpace")]
        public void AddEntToModelSpace()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            DBText text = new DBText();
            //text.Position = new Point3d(0, 0, 0);//错误
            text.Position = new Point3d();
            text.TextString = "Learning1_2 MyText";
            ToModelSpace(text, database);
        }


        /// <summary>
        /// 将图像对象加入指定Database的模型空间
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="database">数据库</param>
        /// <returns>实体ObjectId</returns>
        public static ObjectId ToModelSpace(Entity entity, Database database)
        {
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

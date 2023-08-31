using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.EntityLearn.Learning2_3))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_3
    {
        /// <summary>
        /// 引用外部文件为块定义创建块参照
        /// </summary>
        [CommandMethod("AddRefBlock")]
        public void AddRefBlock()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace],OpenMode.ForWrite);
                //把外部文件转换为块定义
                ObjectId refObjectId = database.OverlayXref("C:\\Users\\AideSoftware\\Desktop\\局部坐标系.dwg", "我的坐标系");
                //通过块定义创建块参照
                BlockReference blockReference = new BlockReference(Point3d.Origin, refObjectId);
                modelSpace.AppendEntity(blockReference);
                transaction.AddNewlyCreatedDBObject(blockReference, true);
                transaction.Commit();
            }
        }

        /// <summary>
        /// 块定义创建块参照
        /// </summary>
        [CommandMethod("AddBlock")]
        public void AddBlock()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            ObjectId MyBlockId;
            //创建块定义
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId,OpenMode.ForWrite);
                BlockTableRecord myBlock = new BlockTableRecord();
                myBlock.Name = "MyBlock";
                Line line = new Line(Point3d.Origin,new Point3d(1000,1000,0));
                Circle circle = new Circle(Point3d.Origin, Vector3d.ZAxis, 500);
                myBlock.AppendEntity(line);
                myBlock.AppendEntity(circle);
                MyBlockId = blockTable.Add(myBlock);
                transaction.AddNewlyCreatedDBObject(myBlock, true);
                transaction.Commit();
            }

            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                //通过块定义创建块参照
                BlockReference blockReference = new BlockReference(new Point3d(0, 0, 0), MyBlockId);
                modelSpace.AppendEntity(blockReference);
                transaction.AddNewlyCreatedDBObject(blockReference, true);
                transaction.Commit();
            }
        }
    }
}

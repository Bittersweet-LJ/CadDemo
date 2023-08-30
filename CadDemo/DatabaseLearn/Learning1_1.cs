using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.DatabaseLearn.Learning1_1))]
namespace CadDemo.DatabaseLearn
{
    internal class Learning1_1
    {
        /// <summary>
        /// 添加块定义
        /// </summary>
        [CommandMethod("AddBlockDef")]
        public void AddBlockDef()
        {
            //获取当前数据库
            //Database db = Application.DocumentManager.MdiActiveDocument.Database;
            Database db = HostApplicationServices.WorkingDatabase;

            BlockTableRecord btr = new BlockTableRecord();
            btr.Name = "mybtr";
            Line line = new Line(Point3d.Origin, new Point3d(10, 15, 0));
            Circle circle = new Circle(Point3d.Origin, Vector3d.ZAxis, 10);
            btr.AppendEntity(line);
            btr.AppendEntity(circle);
            AddBlockTableRecord(btr,db);
        }

        /// <summary>
        /// 将块表记录加入块表中
        /// </summary>
        /// <param name="btr">块表记录</param>
        /// <param name="db">数据库</param>
        /// <returns>块表记录ObjectId</returns>
        public ObjectId AddBlockTableRecord(BlockTableRecord btr,Database db)
        {
            ObjectId id = new ObjectId();
            using(Transaction transaction = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = transaction.GetObject(db.BlockTableId,OpenMode.ForWrite) as BlockTable;
                id = bt.Add(btr);
                transaction.AddNewlyCreatedDBObject(btr, true);
                transaction.Commit();
            }
            return id;
        }
    }
}

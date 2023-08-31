using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.EntityLearn.Learning2_4))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_4
    {
        /// <summary>
        /// 添加带属性的块
        /// </summary>
        [CommandMethod("AddAttributeBlock")]
        public void AddAttributeBlock()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            ObjectId objectId;
            BlockTableRecord blockTableRecord = new BlockTableRecord();
            Line line = new Line(new Point3d(), new Point3d(30, 30, 0));
            AttributeDefinition attributeDefinition = new AttributeDefinition()
            {
                Constant = false,
                Tag = "Length",
                Prompt = "L",
                TextString = line.Length.ToString(),
                Position = new Point3d(30, 30, 0),
            };
            blockTableRecord.Name = "MyLineBlock";
            blockTableRecord.AppendEntity(line);
            blockTableRecord.AppendEntity(attributeDefinition);

            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForWrite);
                objectId = blockTable.Add(blockTableRecord);
                transaction.AddNewlyCreatedDBObject(blockTableRecord, true);
                transaction.Commit();
            }

            ToModelSpace(objectId, Point3d.Origin, database);
        }

        /// <summary>
        /// 将指定块定义变成块参照添加到指定模型空间
        /// </summary>
        /// <param name="myBlockId">块定义Id</param>
        /// <param name="point3D">插入点</param>
        /// <param name="database">数据库</param>
        /// <returns>块参照ObjectId</returns>
        private ObjectId ToModelSpace(ObjectId myBlockId,Point3d point3D,Database database)
        {
            ObjectId blockRefId = new ObjectId();
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                BlockReference blockReference = new BlockReference(point3D, myBlockId);
                blockRefId = modelSpace.AppendEntity(blockReference);
                transaction.AddNewlyCreatedDBObject(blockReference,true);
                BlockTableRecord myBlockRecord = (BlockTableRecord)transaction.GetObject(myBlockId, OpenMode.ForRead);

                foreach(ObjectId id in myBlockRecord)
                {
                    if (id.ObjectClass.Equals(RXClass.GetClass(typeof(AttributeDefinition))))
                    {
                        AttributeDefinition attributeDefinition = (AttributeDefinition)transaction.GetObject(id,OpenMode.ForRead);
                        AttributeReference attributeReference = new AttributeReference(attributeDefinition.Position, attributeDefinition.TextString, attributeDefinition.Tag, new ObjectId());
                        blockReference.AttributeCollection.AppendAttribute(attributeReference);
                    }
                }
                transaction.Commit();
            }
            return blockRefId;
        }
    }
}

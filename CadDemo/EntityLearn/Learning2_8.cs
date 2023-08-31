using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: CommandClass(typeof(CadDemo.EntityLearn.Learning2_8))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_8
    {

        /// <summary>
        /// 改变显示次序
        /// </summary>
        [CommandMethod("ToTop")]
        public void ToTop()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                Entity entity = SelectEntity("\n请选择要前置的对象");
                MoveTop(entity, database);
            }
        }

        /// <summary>
        /// 改变对象绘图次序到顶层
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="database"></param>
        private void MoveTop(Entity entity, Database database)
        {
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                ObjectIdCollection objectIdCollection = new ObjectIdCollection();
                objectIdCollection.Add(entity.ObjectId);
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForWrite);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                DrawOrderTable drawOrderTable = (DrawOrderTable)transaction.GetObject(modelSpace.DrawOrderTableId, OpenMode.ForWrite);
                drawOrderTable.MoveToTop(objectIdCollection);
                transaction.Commit();
            }
        }


        /// <summary>
        /// 提示用户选择单个实体
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Entity SelectEntity(string message)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;
            Entity entity = null;
            PromptEntityResult entityResult = editor.GetEntity(message);
            if (entityResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    entity = (Entity)transaction.GetObject(entityResult.ObjectId, OpenMode.ForWrite);
                    transaction.Commit();
                }
            }
            return entity;

        }
    }
}

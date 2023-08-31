using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.DatabaseLearn.Learning1_5))]
namespace CadDemo.DatabaseLearn
{
    internal class Learning1_5
    {
        /// <summary>
        /// 写块克隆
        /// </summary>
        [CommandMethod("WblockClone")]
        public void WblockClone()
        {
            ObjectIdCollection objectIdCollection = new ObjectIdCollection();
            foreach (DBObject obj in GetCollection())
            {
                objectIdCollection.Add(obj.ObjectId);
            }
            WClone(objectIdCollection, "C:\\Users\\AideSoftware\\Desktop\\new.dwg");
        }

        /// <summary>
        /// 获取用户选集
        /// </summary>
        /// <returns>选择集合</returns>
        public static DBObjectCollection GetCollection()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database db = document.Database;
            Editor editor = document.Editor;
            DBObjectCollection entityCollection = new DBObjectCollection();
            PromptSelectionResult promptSelectionResult = editor.GetSelection();
            if(promptSelectionResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet selectionSet = promptSelectionResult.Value;
                    foreach(ObjectId id in selectionSet.GetObjectIds())
                    {
                        Entity entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true); 
                        if(null != entity)
                        {
                            entityCollection.Add(entity);
                        }
                    }
                    transaction.Commit();
                }
            }
            return entityCollection;
        }

        /// <summary>
        /// 写块克隆
        /// </summary>
        /// <param name="idCollection">要克隆的对象ID集合</param>
        /// <param name="fileName">克隆到的目标文件名</param>
        public static void WClone(ObjectIdCollection idCollection,string fileName)
        {
            Database newDatabase = new Database(true,true);
            ObjectId objectId = new ObjectId();
            Database database = idCollection[0].Database;
            IdMapping idMapping = new IdMapping(); 
            using(Transaction transaction = newDatabase.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(newDatabase.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                objectId = modelSpace.ObjectId;
                transaction.Commit();
            }
            database.WblockCloneObjects(idCollection, objectId, idMapping, DuplicateRecordCloning.Replace, false);
            newDatabase.SaveAs(fileName, DwgVersion.Current);
        }
    }
}

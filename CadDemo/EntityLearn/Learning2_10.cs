using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.EntityLearn.Learning2_10))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_10
    {
        /// <summary>
        /// 添加实体到组 
        /// </summary>
        [CommandMethod("EntityGroup")]
        public void EntityGroup()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
                Entity entity1 = SelectEntity("\n选择要添加到组的第一个对象");
                Entity entity2 = SelectEntity("\n选择要添加到组的第二个对象");
                Group group = new Group();
                group.Append(entity1.ObjectId);
                group.Append(entity2.ObjectId);
                DBDictionary groupDict = (DBDictionary)transaction.GetObject(database.GroupDictionaryId, OpenMode.ForWrite);
                group.Selectable = true;
                groupDict.SetAt("MyGroup", group);
                transaction.AddNewlyCreatedDBObject(group, true);
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

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;


[assembly: CommandClass(typeof(CadDemo.JigLearn.Learning5_1))]
namespace CadDemo.JigLearn
{
    internal class Learning5_1
    {
        /// <summary>
        /// 直线即时绘图
        /// </summary>
        [CommandMethod("LineJig")]
        public void MyLineJig()
        {
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            LineJig lineJig = new LineJig();
            PromptResult promptResult = editor.Drag(lineJig);
            if(promptResult.Status == PromptStatus.OK )
            {
                lineJig.SetCount(1);
                promptResult = editor.Drag(lineJig);
                ToModelSpace(lineJig.Entity);
            }
            
        }

        private void ToModelSpace(Entity entity)
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                modelSpace.AppendEntity(entity);
                transaction.AddNewlyCreatedDBObject(entity, true);
                transaction.Commit();
            }
        }
    }
}

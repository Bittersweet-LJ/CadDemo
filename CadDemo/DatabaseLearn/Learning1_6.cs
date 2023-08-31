using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.DatabaseLearn.Learning1_6))]
namespace CadDemo.DatabaseLearn
{
    internal class Learning1_6
    {
        /// <summary>
        /// 设置当前视图(通过editor的SetCurrentView设置)
        /// </summary>
        [CommandMethod("SetCurrentView")]
        public void SetCurrentView()
        {
            Entity entity = SelectEntity("\n请选择一个实体");
            ViewEntity(entity, 5);
        }

        /// <summary>
        /// 选择单个实体
        /// </summary>
        /// <param name="message">选择提示</param>
        /// <returns>实体对象</returns>
        private Entity SelectEntity(string message)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;
            Entity entity = null;
            PromptEntityResult result = editor.GetEntity(message);
            if (result.Status == PromptStatus.OK)
            {
                using(Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    entity = (Entity)transaction.GetObject(result.ObjectId, OpenMode.ForRead,true);
                    transaction.Commit();
                }
            }
            return entity;
        }

        /// <summary>
        /// 根据实体范围设置当前视图
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scale">视图比例</param>
        private void ViewEntity(Entity entity,double scale)
        {
            Database database = entity.Database;
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            using(ViewTableRecord viewTableRecord = new ViewTableRecord())
            {
                Point2d pointMin = new Point2d(entity.GeometricExtents.MinPoint.X, entity.GeometricExtents.MinPoint.Y);
                Point2d pointMax = new Point2d(entity.GeometricExtents.MaxPoint.X, entity.GeometricExtents.MaxPoint.Y);
                viewTableRecord.CenterPoint = new Point2d((pointMin.X+pointMax.X)/2, (pointMin.Y + pointMax.Y) / 2);
                viewTableRecord.Width = Math.Abs(pointMax.X - pointMin.X)*scale;
                viewTableRecord.Height = Math.Abs(pointMax.Y - pointMin.Y)*scale;
                editor.SetCurrentView(viewTableRecord);
            }
        }
    }
}

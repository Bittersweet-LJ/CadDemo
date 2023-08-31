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


[assembly:CommandClass(typeof(CadDemo.EntityLearn.Learning2_5))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_5
    {
        //实体变形：镜像实体
        [CommandMethod("MirrorEntity")]
        public void MirrorEntity()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                Entity entity = SelectEntity("\n选择对象");
                if (entity != null)
                {
                    CreateMirror(entity,new Point3d(0,0,0),new Point3d(0,1,0));
                }
                transaction.Commit();
            }
        }

        /// <summary>
        /// 指定两个镜像参照点得到实体镜像
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="point3d1"></param>
        /// <param name="point3d2"></param>
        private void CreateMirror(Entity entity, Point3d point3d1, Point3d point3d2)
        {
            Line3d mirrorLine = new Line3d(point3d1,point3d2);
            Matrix3d matrix3D = Matrix3d.Mirroring(mirrorLine);
            entity.TransformBy(matrix3D);
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

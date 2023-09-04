using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;


[assembly: CommandClass(typeof(CadDemo.JigLearn.Learning5_2))]
namespace CadDemo.JigLearn
{
    internal class Learning5_2
    {

        /// <summary>
        /// 多段线即时绘图
        /// </summary>
        [CommandMethod("PolyLineJig")]
        public void MyPolyLineJig()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor editor = document.Editor;
            Matrix3d ucs = editor.CurrentUserCoordinateSystem;
            PolyLineJig polyLineJig = new PolyLineJig(ucs);

            bool bPoint;
            bool bKeyword;
            bool bComplete;
            do
            {
                PromptResult promptResult = editor.Drag(polyLineJig);

                bPoint = (promptResult.Status == PromptStatus.OK);
                if (bPoint)
                {
                    polyLineJig.AddDummyVertex();
                }

                bKeyword = (promptResult.Status == PromptStatus.Keyword);
                if (bKeyword)
                {
                    if (polyLineJig.IsUndoing)
                    {
                        polyLineJig.RemoveDummyVertex();
                    }
                    else
                    {
                        polyLineJig.AdjustSegmentType(polyLineJig.IsArcSeg);
                    }
                }

                bComplete = (promptResult.Status == PromptStatus.None);
                if (bComplete)
                {
                    polyLineJig.RemoveDummyVertex();
                }
            }
            while((bPoint || bKeyword) && !bComplete);

            if(bComplete)
            {
                Polyline polyline = polyLineJig.GetEntity() as Polyline;
                if(polyline.NumberOfVertices >1)
                {
                    ToModelSpace(polyline);
                }
            }
        }


        private void ToModelSpace(Entity entity)
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
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

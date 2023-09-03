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

[assembly:CommandClass(typeof(CadDemo.UserInterfaceLearn.Learning3_2))]
namespace CadDemo.UserInterfaceLearn
{
    /// <summary>
    /// 输入数据
    /// </summary>
    internal class Learning3_2
    {

        /// <summary>
        /// 拾取点，输出点坐标
        /// </summary>
        [CommandMethod("PickPoint")]
        public void PickPoint()
        {
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            Point3d point = new Point3d();
            PromptPointResult promptPointResult = editor.GetPoint("\n拾取点");
            if(promptPointResult.Status == PromptStatus.OK)
            {
                point = promptPointResult.Value;
            }
            editor.WriteMessage("\n拾取的点坐标为{0},{1},{2}", point.X, point.Y, point.Z);
        }

        /// <summary>
        /// 选择对象，输出对象ObjectId
        /// </summary>
        [CommandMethod("SelectEntity")]
        public void SelectEntity()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor editor = document.Editor;
            Entity entity = null;
            PromptEntityResult promptEntityResult = editor.GetEntity("\n请选择对象");
            if(promptEntityResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    entity = (Entity)transaction.GetObject(promptEntityResult.ObjectId, OpenMode.ForRead);
                    transaction.Commit();
                }
            }
            editor.WriteMessage("\n你选择的对象ObjectId:" + entity.ObjectId.ToString());
        }

        /// <summary>
        /// 过滤选择
        /// </summary>
        [CommandMethod("Selection")]
        public void Selection()
        {
            FilterType lineType = FilterType.Line;
            FilterType textType = FilterType.DBText;
            FilterType circleType = FilterType.Circle;
            FilterType[] types = new FilterType[3];
            types[0] = lineType;
            types[1] = textType;
            types[2] = circleType;
            DBObjectCollection entityCollection = GetSelection(types);
        }


        /// <summary>
        /// 过滤集合选择
        /// </summary>
        /// <param name="types">过滤类型</param>
        /// <returns>对象集合</returns>
        private DBObjectCollection GetSelection(FilterType[] types)
        {
            Document document= Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;
            Entity entity = null;
            DBObjectCollection entityCollection = new DBObjectCollection();
            PromptSelectionOptions promptSelectionOptions = new PromptSelectionOptions();
            //建立选择的过滤器内容
            TypedValue[] filList = new TypedValue[types.Length+2];
            filList[0] = new TypedValue((int)DxfCode.Operator,"<or");
            filList[types.Length+1] = new TypedValue((int)DxfCode.Operator, "or>");
            for(int i = 0; i < types.Length; i++)
            {
                filList[i+1] = new TypedValue((int)DxfCode.Start, types[i].ToString());
            }
            //建立过滤器
            SelectionFilter filter = new SelectionFilter(filList);
            //按照过滤器进行选择
            PromptSelectionResult promptSelectionResult = editor.GetSelection(promptSelectionOptions, filter);
            if (promptSelectionResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    SelectionSet selectionSet = promptSelectionResult.Value;
                    foreach (ObjectId objectId in selectionSet.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(objectId, OpenMode.ForRead);
                        if (entity != null)
                        {
                            entityCollection.Add(entity);
                        }
                    }
                    transaction.Commit();
                }
            }
            return entityCollection;
        }
    }

    /// <summary>
    /// 类型过滤枚举类
    /// </summary>
    public enum FilterType
    {
        Curve,
        Dimension,
        Polyline,
        BlockRef,
        Circle,
        Line,
        Arc,
        DBText,
        MText,
        PolyLine3d,
        Surface,
        Region,
        Hatch,
        Helix,
        DBPoint
    }
}

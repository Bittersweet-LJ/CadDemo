using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.DatabaseLearn.Learning1_3))]
namespace CadDemo.DatabaseLearn
{
    internal class Learning1_3
    {
        /// <summary>
        /// 添加图层
        /// </summary>
        [CommandMethod("AddLayer")]
        public void AddLayer()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            AddLayerTableRecord("myLayerName",1,database);
        }

        /// <summary>
        /// 删除图层
        /// </summary>
        [CommandMethod("RemoveLayer")]
        public void RemoveLayer()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            RemoveLayerTableRecord("myLayerName", database);
        }

        /// <summary>
        /// 建立指定名字，颜色的图层
        /// </summary>
        /// <param name="layerName">图层名</param>
        /// <param name="colorIndex">颜色索引</param>
        /// <param name="database">数据库</param>
        /// <returns>图层ObjectId</returns>
        public static ObjectId AddLayerTableRecord(string layerName,short colorIndex,Database database)
        {
            colorIndex = (short)(colorIndex % 256);
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)transaction.GetObject(database.LayerTableId, OpenMode.ForWrite);
                ObjectId layerId = ObjectId.Null;
                if (lt.Has(layerName) == false)
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = layerName;
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByColor, colorIndex);
                    layerId = lt.Add(ltr);
                    transaction.AddNewlyCreatedDBObject(ltr, true);
                }
                transaction.Commit();
                return layerId;
            }
        }

        /// <summary>
        /// 删除指定名称的图层
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="database">数据库</param>
        public static void RemoveLayerTableRecord(string layerName,Database database)
        {
            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            using(Transaction transaction = database.TransactionManager.StartTransaction())
            {
                LayerTable layerTable = (LayerTable)transaction.GetObject(database.LayerTableId, OpenMode.ForWrite);
                LayerTableRecord currentLayer = (LayerTableRecord)transaction.GetObject(database.Clayer, OpenMode.ForRead);
                if (currentLayer.Name.ToLower() == layerName.ToLower())
                {
                    editor.WriteMessage("\n不能删除当前层");
                }
                else
                {
                    // 层表中存在，尝试删除
                    if (layerTable.Has(layerName))
                    {
                        LayerTableRecord layerTableRecord = (LayerTableRecord)transaction.GetObject(layerTable[layerName], OpenMode.ForWrite);
                        if(layerTableRecord.IsErased)
                        {
                            editor.WriteMessage("/n该层已被删除");
                        }
                        else
                        {
                            ObjectIdCollection idCollection = new ObjectIdCollection();
                            idCollection.Add(layerTableRecord.ObjectId);
                            database.Purge(idCollection);
                            if(idCollection.Count == 0)
                            {
                                editor.WriteMessage("\n不能删除包含对象的图层");
                            }
                            else
                            {
                                layerTableRecord.Erase();
                            }
                        }
                    }
                    else
                    {
                        editor.WriteMessage("\n没有该图层");
                    }
                    transaction.Commit();
                }
            }
        }
    }
}

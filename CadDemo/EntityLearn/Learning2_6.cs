using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly:CommandClass(typeof(CadDemo.EntityLearn.Learning2_6))]
namespace CadDemo.EntityLearn
{
    //扩展数据 
    internal class Learning2_6
    {
        // 使用DataTable附加数据
        [CommandMethod("WriteData")]
        public void WriteData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "ParameterTable";
            dt.AppendColumn(CellType.CharPtr, "Name");
            dt.AppendColumn(CellType.CharPtr, "Material");
            dt.AppendColumn(CellType.CharPtr, "Parameter");
            DataCellCollection RowDataCellCollection = new DataCellCollection();
            DataCell nameDataCell = new DataCell();
            DataCell materialDataCell = new DataCell();
            DataCell parameterDataCell = new DataCell();
            nameDataCell.SetString("工字钢");
            materialDataCell.SetString("Q235B");
            parameterDataCell.SetString("200*200*32*25");
            RowDataCellCollection.Add(nameDataCell);
            RowDataCellCollection.Add(materialDataCell);
            RowDataCellCollection.Add(parameterDataCell);
            dt.AppendRow(RowDataCellCollection, true);


            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;
            PromptEntityResult entityResult = editor.GetEntity("\n选择要写数据的对象");
            if (entityResult.Status == PromptStatus.OK)
            {
                using(Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    Entity entity = (Entity)transaction.GetObject(entityResult.ObjectId, OpenMode.ForWrite, true);
                    if(entity.ExtensionDictionary == new ObjectId())
                    {
                        entity.CreateExtensionDictionary();
                    }
                    DBDictionary extensionDict = (DBDictionary)transaction.GetObject(entity.ExtensionDictionary, OpenMode.ForWrite, false);
                    extensionDict.SetAt("ParameterTable", dt);
                    transaction.AddNewlyCreatedDBObject(dt, true);
                    transaction.Commit();
                }
            }
        }


        [CommandMethod("ReadData")]
        public void ReadData()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;
            Entity entity = null;
            PromptEntityResult entityResult = editor.GetEntity("\n选择要读数据的对象");
            if (entityResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = document.TransactionManager.StartTransaction())
                {
                    entity = (Entity)transaction.GetObject(entityResult.ObjectId, OpenMode.ForRead, true);
                    DBDictionary extensionDict = (DBDictionary)transaction.GetObject(entity.ExtensionDictionary, OpenMode.ForRead);
                    DataTable dataTable = (DataTable)transaction.GetObject(extensionDict.GetAt("ParameterTable"), OpenMode.ForWrite, true);
                    editor.WriteMessage("\n Name:" + dataTable.GetCellAt(0, 0).Value.ToString());
                    editor.WriteMessage("\n Matarial:" + dataTable.GetCellAt(0, 1).Value.ToString());
                    editor.WriteMessage("\n Parameter:" + dataTable.GetCellAt(0, 2).Value.ToString());
                    DataColumn nameDataColumn = dataTable.GetColumnAt(0);
                    DataColumn matarialDataColumn = dataTable.GetColumnAt(1);
                    DataColumn parameterDataColumn = dataTable.GetColumnAt(2);
                    editor.WriteMessage("\n Name:"+nameDataColumn.GetCellAt(0).Value.ToString());
                    editor.WriteMessage("\n Matarial:" + matarialDataColumn.GetCellAt(0).Value.ToString());
                    editor.WriteMessage("\n Parameter:" + parameterDataColumn.GetCellAt(0).Value.ToString());
                    transaction.Commit();
                }
            }
        }
    }
}

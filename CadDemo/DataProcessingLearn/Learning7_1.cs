using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.DataProcessingLearn.Learning7_1))]
namespace CadDemo.DataProcessingLearn
{
    internal class Learning7_1
    {
        /// <summary>
        /// 写 Excel 文件
        /// </summary>
        [CommandMethod("ToExcel")]
        public void ToExcel()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.TableName = "LineTable";
            dataTable.Columns.Add("Handle",typeof(string));
            dataTable.Columns.Add("StartPointX",typeof(double));
            dataTable.Columns.Add("StartPointY",typeof(double));
            dataTable.Columns.Add("StartPointZ",typeof(double));
            dataTable.Columns.Add("EndPointX",typeof(double));
            dataTable.Columns.Add("EndPointY", typeof(double));
            dataTable.Columns.Add("EndPointZ", typeof(double));
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                foreach (ObjectId objectId in modelSpace)
                {
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    Line line = (Line)transaction.GetObject(objectId, OpenMode.ForRead);
                    if(line != null)
                    {
                        dataRow[0] = line.Handle.ToString();
                        dataRow[1] = line.StartPoint.X;
                        dataRow[2] = line.StartPoint.Y;
                        dataRow[3] = line.StartPoint.Z;
                        dataRow[4] = line.EndPoint.X;
                        dataRow[5] = line.EndPoint.Y;
                        dataRow[6] = line.EndPoint.Z;
                    }
                    dataTable.Rows.Add(dataRow);
                }
                transaction.Commit();
            }
            SaveTo(dataTable, "C:\\Users\\Bittersweet\\Desktop\\data_learning7_1.xlsx");
        }

        /// <summary>
        /// 把数据保存到Excel文件
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="fileName"></param>
        private void SaveTo(System.Data.DataTable dataTable, string fileName)
        {
            int columnIndex = 1;
            int rowIndex = 1;
            Microsoft.Office.Interop.Excel.Application excelApplication = new Microsoft.Office.Interop.Excel.Application();
            excelApplication.DefaultFilePath = fileName;
            excelApplication.DisplayAlerts = true;
            excelApplication.SheetsInNewWorkbook = 1;
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApplication.Workbooks.Add(true);
            //将DataTable的列名导入Excel表第一行
            foreach (System.Data.DataColumn column in dataTable.Columns)
            {
                excelApplication.Cells[rowIndex,columnIndex] = column.ColumnName;
                columnIndex++;
            }
            //将DataTable中的数据导入Excel中
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                columnIndex = 1;
                rowIndex++;
                for(int j = 0;j< dataTable.Columns.Count; j++)
                {
                    excelApplication.Cells[rowIndex,columnIndex] = dataTable.Rows[i][j].ToString();
                    columnIndex++;
                }
            }
            workbook.SaveCopyAs(fileName);
            excelApplication = null;
            workbook = null;
        }
    }
}

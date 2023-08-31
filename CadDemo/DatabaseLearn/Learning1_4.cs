using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.DatabaseLearn.Learning1_4))]
namespace CadDemo.DatabaseLearn
{
    internal class Learning1_4
    {
        /// <summary>
        /// 添加对象到命名对象词典
        /// </summary>
        [CommandMethod("AddObjToNod")]
        public void AddObiToNod()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            DataTable dataTable = new DataTable();
            dataTable.TableName = "MyTable";
            ToNod(dataTable,"MyDataTable", database);
        }

        /// <summary>
        /// 将一个对象添加到命名词典
        /// </summary>
        /// <param name="dataObi">要添加的对象</param>
        /// <param name="name">词典记录名</param>
        /// <param name="database">数据库</param>
        public static void ToNod(DBObject dataObi,string name,Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                DBDictionary NOdict = (DBDictionary)transaction.GetObject(database.NamedObjectsDictionaryId, OpenMode.ForWrite);
                NOdict.SetAt(name, dataObi);
                transaction.Commit();
            }
        }


    }
}

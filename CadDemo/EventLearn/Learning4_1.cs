using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.EventLearn.Learning4_1))]
namespace CadDemo.EventLearn
{
    /// <summary>
    /// 对象删除事件
    /// </summary>
    internal class Learning4_1
    {
        [CommandMethod("AddObjectErasedEvent")]
        public void AddObjectErasedEvent()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            database.ObjectErased += new ObjectErasedEventHandler(MyObjectErased);
        }

        

        [CommandMethod("RemoveObjectErasedEvent")]
        public void RemoveObjectErasedEvent()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            database.ObjectErased -= new ObjectErasedEventHandler(MyObjectErased);
        }

        private void MyObjectErased(object sender, ObjectErasedEventArgs e)
        {
            Application.ShowAlertDialog("\n删除的对象ObjectId为:" + e.DBObject.ObjectId.ToString());
        }
    }
}

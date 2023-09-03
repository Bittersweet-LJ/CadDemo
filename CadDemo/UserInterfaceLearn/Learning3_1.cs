using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly:CommandClass(typeof(CadDemo.UserInterfaceLearn.Learning3_1))]
namespace CadDemo.UserInterfaceLearn
{
    /// <summary>
    /// 输出消息
    /// </summary>
    internal class Learning3_1
    {
        [CommandMethod("EditorMessage")]
        public void EditorMessage()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor editor = document.Editor;
            editor.WriteMessage("MyEditorMessage");
        }

        [CommandMethod("AlertMessage")]
        public void AlertMessage()
        {
            Application.ShowAlertDialog("MyAlertMessage");
        }

        [CommandMethod("WebMessage")]
        public void ShowWeb()
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://www.bittersweet.love");
        }
    }
}

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using CadDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.UserInterfaceLearn.Learning3_3))]
namespace CadDemo.UserInterfaceLearn
{
    /// <summary>
    /// 自定义用户界面
    /// </summary>
    internal class Learning3_3
    {
        /// <summary>
        /// 模态对话框
        /// </summary>
        [CommandMethod("ModalDialog")]
        public void ShowModalDilog()
        {
            MyDialogWindow myDialogWindow = new MyDialogWindow();
            myDialogWindow.ShowInTaskbar = false;
            Application.ShowModalWindow(myDialogWindow);
            if(myDialogWindow.DialogResult == true)
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n" + myDialogWindow.PointText.Text);
            }

        }

        /// <summary>
        /// 非模态对话框
        /// </summary>
        [CommandMethod("ModelessDialog")]
        public void ShowModelessDilog()
        {
            MyDialogWindow myDialogWindow = new MyDialogWindow();
            myDialogWindow.ShowInTaskbar = false;
            Application.ShowModelessWindow(myDialogWindow);
        }
    }
}

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.UserInterfaceLearn.Learning3_6))]
namespace CadDemo.UserInterfaceLearn
{
    /// <summary>
    /// 快捷菜单
    /// </summary>
    internal class Learning3_6
    {
        [CommandMethod("AddContextMenu")]
        public void AddContextMenu()
        {
            ContextMenuExtension contextMenuExtension = new ContextMenuExtension();
            contextMenuExtension.Title = "快捷菜单";
            MenuItem menuItem_createLine = new MenuItem("test创建线");
            menuItem_createLine.Click += new EventHandler(MenuItem_CreateLine_Click);
            MenuItem menuItem_createCircle = new MenuItem("test创建圆");
            menuItem_createCircle.Click += new EventHandler(MenuItem_CreateCircle_Click);
            contextMenuExtension.MenuItems.Add(menuItem_createLine);
            contextMenuExtension.MenuItems.Add(menuItem_createCircle);
            Application.AddDefaultContextMenuExtension(contextMenuExtension);

        }


        private void MenuItem_CreateLine_Click(object sender, EventArgs e)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            document.SendStringToExecute("Line\n", true, false, true);
        }

        private void MenuItem_CreateCircle_Click(object sender, EventArgs e)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            document.SendStringToExecute("Circle\n", true, false, true);
        }


    }
}

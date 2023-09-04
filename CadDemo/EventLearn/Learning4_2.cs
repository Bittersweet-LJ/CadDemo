using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.EventLearn.Learning4_2))]
namespace CadDemo.EventLearn
{
    /// <summary>
    /// 文档双击事件
    /// </summary>
    internal class Learning4_2
    {
        bool IsMyDoubleClickEvent = false;

        [CommandMethod("AddDoubleClickEvent")]
        public void AddDoubleClickEvent()
        {
            Application.DocumentManager.DocumentLockModeChanged += new DocumentLockModeChangedEventHandler(My_DocumentLockModeChanged);
            Application.BeginDoubleClick += new BeginDoubleClickEventHandler(My_BeginDoubleClick);
        }

        [CommandMethod("RemoveDoubleClickEvent")]
        public void RemoveDoubleClickEvent()
        {
            Application.DocumentManager.DocumentLockModeChanged -= new DocumentLockModeChangedEventHandler(My_DocumentLockModeChanged);
            Application.BeginDoubleClick -= new BeginDoubleClickEventHandler(My_BeginDoubleClick);
        }


        private void My_BeginDoubleClick(object sender, BeginDoubleClickEventArgs e)
        {
            IsMyDoubleClickEvent = true;
        }

        private void My_DocumentLockModeChanged(object sender, DocumentLockModeChangedEventArgs e)
        {
            if(e.GlobalCommandName.ToLower() == "quickproperties")
            {
                if(IsMyDoubleClickEvent)
                {
                    e.Veto();
                    Document document = Application.DocumentManager.MdiActiveDocument;
                    PromptSelectionResult promptSelectionResult = document.Editor.SelectImplied();
                    if(promptSelectionResult.Value.Count != 1)
                    {
                        Application.ShowAlertDialog("请选中一个对象");
                        return;
                    }
                    ObjectId objectId = promptSelectionResult.Value[0].ObjectId;
                    Application.ShowAlertDialog("你选中的对象ObjectId为：" + objectId.ToString());
                    IsMyDoubleClickEvent = false;
                }
            }
        }
    }
}

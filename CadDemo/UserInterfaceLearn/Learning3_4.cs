using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using CadDemo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.UserInterfaceLearn.Learning3_4))]
namespace CadDemo.UserInterfaceLearn
{
    internal class Learning3_4
    {
        /// <summary>
        /// 添加AutoCAD自定义面板
        /// </summary>
        [CommandMethod("AddPalette")]
        public void AddPalette()
        {
            MyUserControl myUserControl = new MyUserControl();
            
            PaletteSet paletteSet = new PaletteSet("MyPaletteSet")
            {
                Visible = true,
                Style = PaletteSetStyles.ShowAutoHideButton,
                Dock = DockSides.None,
                MinimumSize = new System.Drawing.Size(200, 100),
                Size = new System.Drawing.Size(200, 100)
            };
            paletteSet.AddVisual("MyPaletteSet", myUserControl);
        }

    }
}

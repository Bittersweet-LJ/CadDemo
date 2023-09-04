using Autodesk.AutoCAD.EditorInput;
using CadApplicationServices = Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CadDemo.Views
{
    /// <summary>
    /// MyDialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MyDialogWindow : Window
    {
        public MyDialogWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Editor editor = CadApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            using (EditorUserInteraction editorUserInteraction = editor.StartUserInteraction(this))
            {
                PromptPointResult promptPointResult = editor.GetPoint("\n选择点");
                Point3d point = new Point3d();
                if (promptPointResult.Status == PromptStatus.OK)
                {
                    point = promptPointResult.Value;
                }
                this.PointText.Text = "("+point.X.ToString()+","+point.Y.ToString()+")";
                editorUserInteraction.End();
                this.Focus();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true; 
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

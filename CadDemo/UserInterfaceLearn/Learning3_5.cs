using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

[assembly: CommandClass(typeof(CadDemo.UserInterfaceLearn.Learning3_5))]
namespace CadDemo.UserInterfaceLearn
{
    /// <summary>
    /// 带式菜单
    /// </summary>
    internal class Learning3_5
    {
        [CommandMethod("AddRibbon")]
        public void AddRibbon()
        {
            RibbonControl ribbonControl = getRibbonControl();
            RibbonTab ribbonTab = createRibbonTab("菜单标签", "1");
            RibbonPanel ribbonPanel = createRibbonPanel("菜单面板");
            RibbonButton ribbonButton = createRibbonButton("菜单按钮", "菜单命令");

            ribbonPanel.Source.Items.Add(ribbonButton);
            ribbonTab.Panels.Add(ribbonPanel);
            ribbonControl.Tabs.Add(ribbonTab);
        }

        /// <summary>
        /// 创建菜单按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="commandParameter"></param>
        /// <returns></returns>
        private RibbonButton createRibbonButton(string text,string commandParameter)
        {
            RibbonButton ribbonButton = new RibbonButton();
            ribbonButton.Text = text;
            ribbonButton.CommandParameter = commandParameter;
            ribbonButton.ShowText = true;
            ribbonButton.CommandHandler = new MyRibbonButtonCommandHandler();
            return ribbonButton;
        }



        /// <summary>
        /// 创建菜单面板
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private RibbonPanel createRibbonPanel(string title)
        {
            RibbonPanelSource ribbonPanelSource = new RibbonPanelSource();
            ribbonPanelSource.Title = title;
            RibbonPanel ribbonPanel = new RibbonPanel();
            ribbonPanel.Source = ribbonPanelSource;
            return ribbonPanel;

        }


        /// <summary>
        /// 创建菜单标签
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private RibbonTab createRibbonTab(string title,string id)
        {
            RibbonTab ribbonTab =  new RibbonTab();
            ribbonTab.Title = title;
            ribbonTab.Id = id;
            ribbonTab.IsActive = true;
            return ribbonTab;
        }

        /// <summary>
        /// 获得菜单控制
        /// </summary>
        /// <returns></returns>
        private RibbonControl getRibbonControl()
        {
            if(null == ComponentManager.Ribbon)
            {
                ComponentManager.ItemInitialized += new EventHandler<RibbonItemEventArgs>(myComponentManagerItemInit);
            }
            return ComponentManager.Ribbon;
        }

        /// <summary>
        /// 用于激活菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myComponentManagerItemInit(object sender, RibbonItemEventArgs e)
        {
            if(ComponentManager.Ribbon != null)
            {
                ComponentManager.ItemInitialized -= new EventHandler<RibbonItemEventArgs> (myComponentManagerItemInit);
            }
        }
    }

    internal class MyRibbonButtonCommandHandler : ICommand
    {
        /// <summary>
        /// 改变可执行状态
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 是否可执行(接口)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            //RibbonButton
            RibbonButton ribbonButton = parameter as RibbonButton;
            if(ribbonButton != null )
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute((string)ribbonButton.CommandParameter, true, false, true);
            }

            //RibbonTextBox
            RibbonTextBox ribbonTextBox = parameter as RibbonTextBox;
            if(ribbonTextBox != null )
            {
                System.Windows.Forms.MessageBox.Show(ribbonTextBox.TextValue);
            }
        }
    }
}

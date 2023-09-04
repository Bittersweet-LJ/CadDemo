using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:CommandClass(typeof(CadDemo.OverruleLearn.Learning6_1))]
namespace CadDemo.OverruleLearn
{
    /// <summary>
    /// 显示重定义 Todo: 目前运行会显示致命错误
    /// </summary>
    internal class Learning6_1
    {
        [CommandMethod("LineToPipe")]
        public void MyLineToPipe()
        {
            LineToPipe line = new LineToPipe(10);
            StartOverRule(typeof(Line), line);
        }

        [CommandMethod("CloseLineToPipe")]
        public void CloseLineToPipe()
        {
            CancelOverRule();
        }



        /// <summary>
        /// 启用重定义
        /// </summary>
        /// <param name="type"></param>
        /// <param name="line"></param>
        private void StartOverRule(Type type, Overrule overrule)
        {
            RXClass CADClass = RXClass.GetClass(type);
            Overrule.AddOverrule(CADClass, overrule,false);
            Overrule.Overruling = true;
        }

        /// <summary>
        /// 关闭重定义
        /// </summary>
        private void CancelOverRule()
        {
            Overrule.Overruling = false;
        }
    }
}

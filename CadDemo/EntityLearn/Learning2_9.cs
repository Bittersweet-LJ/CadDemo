using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(CadDemo.EntityLearn.Learning2_9))]
namespace CadDemo.EntityLearn
{
    internal class Learning2_9
    {
        /// <summary>
        /// 驱动动态块
        /// </summary>
        [CommandMethod("SetDynamicBlockValue")]
        public void SetDynamicBlockValue()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockReference blockReference = (BlockReference)SelectEntity("\n请选择动态块");
                Property[] properties = new Property[]
                {
                    new Property("Width",80),
                    new Property("Height",100)
                };
                SetDynamicValue(blockReference,properties);
                transaction.Commit();
            }
        }

        /// <summary>
        /// 设置动态块属性
        /// </summary>
        /// <param name="blockReference">要设置属性的动态块</param>
        /// <param name="properties">属性数组</param>
        private void SetDynamicValue(BlockReference blockReference, Property[] properties)
        {
            if(blockReference.IsDynamicBlock)
            {
                foreach(DynamicBlockReferenceProperty property in blockReference.DynamicBlockReferencePropertyCollection)
                {
                    for(int i = 0; i < properties.Length; i++)
                    {
                        if (property.PropertyName == properties[i].PropertyName)
                        {
                            property.Value = properties[i].Value;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 提示用户选择单个实体
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Entity SelectEntity(string message)
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Database database = document.Database;
            Editor editor = document.Editor;
            Entity entity = null;
            PromptEntityResult entityResult = editor.GetEntity(message);
            if (entityResult.Status == PromptStatus.OK)
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    entity = (Entity)transaction.GetObject(entityResult.ObjectId, OpenMode.ForWrite);
                    transaction.Commit();
                }
            }
            return entity;
        }
    }


    /// <summary>
    /// 动态块属性
    /// </summary>
    class Property
    {
        public Property(string propertyName,double value)
        {
            _propertyName = propertyName;
            _value = value;
        }
        private string _propertyName;
        public string PropertyName
        {
            get { return _propertyName; }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
        }


    }
}

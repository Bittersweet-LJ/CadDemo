using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace CadDemo.JigLearn
{
    public class LineJig : EntityJig
    {
        Line myLine;
        Point3d startPoint;
        Point3d endPoint;
        int count;

        public LineJig() : base(new Line())
        {
            myLine = new Line();
            count = 0;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions jigPromptPointOptions = new JigPromptPointOptions();
            jigPromptPointOptions.UserInputControls = (UserInputControls.Accept3dCoordinates | UserInputControls.NoZeroResponseAccepted |
                UserInputControls.NoNegativeResponseAccepted);
            jigPromptPointOptions.UseBasePoint = false;
            jigPromptPointOptions.DefaultValue = new Point3d();
            if(count == 0)
            {
                jigPromptPointOptions.Message = "\n选择起点";
                PromptPointResult promptPointResult = prompts.AcquirePoint(jigPromptPointOptions);

                if(promptPointResult.Status == PromptStatus.OK)
                {
                    startPoint = promptPointResult.Value;
                    endPoint = promptPointResult.Value;
                    return SamplerStatus.OK;
                }
            }
            if(count == 1)
            {
                jigPromptPointOptions.Message = "\n选择终点";
                PromptPointResult promptPointResult = prompts.AcquirePoint(jigPromptPointOptions);
                if(promptPointResult.Status == PromptStatus.OK)
                {
                    endPoint = promptPointResult.Value;
                    return SamplerStatus.OK;
                }
                else if(promptPointResult.Status == PromptStatus.Cancel)
                {
                    return SamplerStatus.Cancel;
                }
            }
            return SamplerStatus.Cancel;
        }

        protected override bool Update()
        {
            ((Line)Entity).StartPoint = startPoint;
            ((Line)Entity).EndPoint = endPoint;
            return true;
        }

        public void SetCount(int count)
        {
            this.count = count;
        }

        public new Entity Entity
        {
            get { return base.Entity; }
        }
        
    }
}

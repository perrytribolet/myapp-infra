using Amazon.CDK;
using Constructs;

namespace MyappInfra
{
    // A Stage groups the stacks that deploy together into one environment (Dev, Prod, ...).
    public class AppStage : Stage
    {
        public AppStage(Construct scope, string id, IStageProps props = null) : base(scope, id, props)
        {
            new AppStack(this, "AppStack");
        }
    }
}
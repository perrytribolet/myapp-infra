using Amazon.CDK;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.Pipelines;
using Constructs;

namespace MyappInfra
{
    public class PipelineStack : Stack
    {
        public PipelineStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var pipeline = new CodePipeline(this, "Pipeline", new CodePipelineProps
            {
                PipelineName = "MyappInfraPipeline",

                Synth = new ShellStep("Synth", new ShellStepProps
                {
                    // Must match your GitHub owner/repo and branch exactly.
                    Input = CodePipelineSource.Connection("perrytribolet/myapp-infra", "main", new ConnectionSourceOptions
                    {
                        // Create this connection in us-east-2 (CodePipeline > Settings > Connections).
                        ConnectionArn = "arn:aws:codeconnections:us-east-2:876443017043:connection/c398d8c7-d128-4db7-a433-4deb573030ef"
                    }),
                    // cdk.json's "app" setting runs `dotnet run`, which builds the project during synth.
                    Commands = new[]
                    {
                        "npm install -g aws-cdk",
                        "cdk synth"
                    }
                }),

                // Standard 7.0 image includes the .NET 8 SDK.
                CodeBuildDefaults = new CodeBuildOptions
                {
                    BuildEnvironment = new BuildEnvironment
                    {
                        BuildImage = LinuxBuildImage.STANDARD_7_0
                    }
                }
            });

            var env = new Amazon.CDK.Environment
            {
                Account = "876443017043",
                Region = "us-east-2"
            };

            pipeline.AddStage(new AppStage(this, "Dev", new StageProps { Env = env }));

            // Later, add a Prod stage that waits for your approval in the console:
            // pipeline.AddStage(new AppStage(this, "Prod", new StageProps { Env = env }), new AddStageOpts
            // {
            //     Pre = new Step[] { new ManualApprovalStep("ApproveProd") }
            // });
        }
    }
}
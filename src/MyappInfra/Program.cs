using Amazon.CDK;

namespace MyappInfra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            // Deploy only the pipeline stack by hand (once). The pipeline deploys everything else.
            new PipelineStack(app, "MyappInfraPipelineStack", new StackProps
            {
                Env = new Amazon.CDK.Environment
                {
                    Account = "876443017043",
                    Region = "us-east-2"
                }
            });

            app.Synth();
        }
    }
}
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AwsApigatewayv2Integrations;
using Constructs;

namespace MyappInfra
{
    public class AppStack : Stack
    {
        public AppStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // Builds app/Dockerfile into a container image and runs it on Lambda.
            var fn = new DockerImageFunction(this, "WebFunction", new DockerImageFunctionProps
            {
                Code = DockerImageCode.FromImageAsset("app"),
                MemorySize = 512,
                Timeout = Duration.Seconds(30),
                Architecture = Architecture.X86_64
            });

            // Public HTTPS endpoint that sends every request to the function.
            var api = new HttpApi(this, "WebApi", new HttpApiProps
            {
                DefaultIntegration = new HttpLambdaIntegration("WebIntegration", fn)
            });

            new CfnOutput(this, "ApiUrl", new CfnOutputProps { Value = api.Url });
        }
    }
}
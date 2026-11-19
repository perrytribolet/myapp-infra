using Amazon.CDK;
using Constructs;

namespace MyappInfra
{
    // Your actual resources (Lambda, API Gateway, DynamoDB, etc.) go here.
    public class AppStack : Stack
    {
        public AppStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // Example:
            // new Amazon.CDK.AWS.S3.Bucket(this, "DataBucket");
        }
    }
}
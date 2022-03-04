using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbInitialInfra
{
    sealed class Program
    {
        /*=========Define the CDK environments===========*/
        //A CDK environment is the target AWS account and region into which the stack is intended to be deployed
        //Non Prod
        private const string ENV_NON_PROD = "NONPROD";

        //Prod
        //private const string ENV_PROD = "PROD";
        /*=========END===========*/

        /*=========Define the CDK stages in the environment===========*/
        //Non Prod
        private const string STAGE_DEV = "DEV";
        private const string DEV_VPC_NAME = "EbUI-vpc";
        //private const string STAGE_QA = "QA";
        //private const string STAGE_UAT = "UAT";

        //Prod
        //private const string STAGE_PROD = "PROD";
        //private const string STAGE_SANDBOX = "SANDBOX";
        /*=========END===========*/


        public static void Main(string[] args)
        {
            var app = new App();

            //The below  CDK environment varaibles are set based on the AWS profile specified using the --profile option in the AWS CDK CLI,
            //or the default AWS profile if you don't specify one.
            Amazon.CDK.Environment setEnv(string account, string region)
            {
                return new Amazon.CDK.Environment
                {
                    Account = account,
                    Region = region
                };
            }

            var envNonProd = setEnv(account: "769749008949", region: "us-east-1");

            /*Setting the stages for NON-PROD AWS account*/
            /*=================BEGIN==============================*/
            //Development stage

            var dev = new EbInitialInfraStack(app, $"EbInitialInfraStack-{ENV_NON_PROD}-{STAGE_DEV}", new AStackProps
            {
                Env = envNonProd,
                EnvironmentName = ENV_NON_PROD,
                StageName = STAGE_DEV,
                VpcName = DEV_VPC_NAME
            });

            Tags.Of(dev).Add("Environment", $"{ENV_NON_PROD}");
            Tags.Of(dev).Add("StageName", $"{STAGE_DEV}");
            Tags.Of(dev).Add("CreatedBy", "AWSCDK");
            Tags.Of(dev).Add("ForApplication", "EventBroker");


            /*Setting the stages for PROD AWS account*/
            /*=================BEGIN==============================*/
            //Prod stage
            //var prod = new EbInitialInfraStack(app, $"EbInitialInfraStack-{ENV_PROD}-{STAGE_PROD}", new AStackProps
            //{
            //    Env = setEnv(),
            //    EnvironmentName = ENV_PROD,
            //    StageName = STAGE_PROD

            //});

            app.Synth();
        }
    }
}

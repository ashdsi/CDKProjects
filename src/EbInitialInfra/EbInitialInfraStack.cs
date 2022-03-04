using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.ECS.Patterns;
using Constructs;


namespace EbInitialInfra
{
    public class EbInitialInfraStack : Stack
    {
        internal EbInitialInfraStack(Construct scope, string id, IAStackProps props = null) : base(scope, id, props)
        {
            string vpcName = props.VpcName;
            //string envName = props.EnvironmentName;
            string stageName = props.StageName;
            string appNameUI = "EbUI";
            //string appNameAPI = "EbApp";

            //Reference existing VPC for DEV environment to be used for Event Broker
            var vpc = Vpc.FromLookup(this, "EbUIVpc", new VpcLookupOptions
            { 
               VpcName = vpcName // Default is all AZs in region
            });

            //Create ECS cluster
            var cluster = new Cluster(this, "EbUIEcs", new ClusterProps
            {
                
                ClusterName = $"{appNameUI}-ECS-{stageName}",
                Vpc = vpc
            });

            //Create a new Security Group for ALB and add ingress rule
            var albsg = new SecurityGroup(this, "EbUIAlbSg", new SecurityGroupProps
            {
                SecurityGroupName = $"{appNameUI}-ALBSG-{stageName}",
                Vpc = vpc,
                AllowAllOutbound = true

            });


            albsg.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(80), "Allow inbound on Port 80");


            //Create ALB 
            var alb = new ApplicationLoadBalancer(this, "EbUIAlb", new Amazon.CDK.AWS.ElasticLoadBalancingV2.ApplicationLoadBalancerProps
            {
                LoadBalancerName = $"{appNameUI}-ALB-{stageName}",
                Vpc = vpc,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PUBLIC
                },
                InternetFacing = true,
                SecurityGroup = albsg
              
            });



            //Add Ingress rule to security group
            
            //Attach Security Group to ALB
            //alb.AddSecurityGroup(albsg);   
            

        }
    }
}

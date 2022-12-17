using System.Net;
using FluentAssertions;
using MbDotNet;
using NUnit.Framework;
using RestSharp;

namespace Shelly.Net.Tests.Integration
{
    /// <summary>
    /// Contains all test dependencies that should be initialized once and only once
    /// before any tests are run
    /// </summary>
    [SetUpFixture]
    internal class Global
    {
        public static MountebankClient MountebankClient { get; private set; }     
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            VerifyMontebankRunning();
            
            MountebankClient = new MountebankClient();
            MountebankClient.DeleteImposter(8095);
        }

        private void VerifyMontebankRunning()
        {
            RestClient client = new RestClient("http://localhost:2525");
            var response = client.Get(new RestRequest("imposters"));
            response.StatusCode.Should().Be(HttpStatusCode.OK, "Montebank must be running. Ensure docker-compose up has been run");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
           
        }
    }
}
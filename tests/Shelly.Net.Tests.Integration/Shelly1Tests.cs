using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MbDotNet.Models.Imposters;
using NrjSolutions.Shelly.Net;
using NUnit.Framework;

namespace Shelly.Net.Tests.Integration
{
    public class Shelly1Tests
    {
        private HttpImposter _imposter;

        [SetUp]
        public void SetUp()
        {
            Global.MountebankClient.DeleteImposter(8095);
            _imposter = Global.MountebankClient.CreateHttpImposter(8095);
        }
        
        [Test]
        public async Task Shelly1_WhenResponseIsInternalServerError_ShouldReturnFailure()
        {
            _imposter.AddStub().OnPathEquals("/status").ReturnsStatus(HttpStatusCode.InternalServerError);
            
            Global.MountebankClient.Submit(_imposter);
            
            Settings settings = Settings.IntegrationTest();

            var sut = new Shelly1Client(settings.User,settings.Password, new Uri(settings.ShellyUri));

            // act
            var shellyResult = await sut.GetStatus(timeout: TimeSpan.FromSeconds(1), cancellationToken: CancellationToken.None);
            
            // assert
            shellyResult.IsSuccess.Should().BeFalse();
            shellyResult.IsFailure.Should().BeTrue();
            shellyResult.IsTransient.Should().BeFalse();
        }
        
        [Test]
        public async Task Shelly1_WhenResponseServiceUnavailable_ShouldReturnTransientFailure()
        {
            _imposter.AddStub().OnPathEquals("/status").ReturnsStatus(HttpStatusCode.ServiceUnavailable);
            
            Global.MountebankClient.Submit(_imposter);
            
            Settings settings = Settings.IntegrationTest();

            var sut = new Shelly1Client(settings.User,settings.Password, new Uri(settings.ShellyUri));

            // act
            var shellyResult = await sut.GetStatus(timeout: TimeSpan.FromSeconds(1), cancellationToken: CancellationToken.None);
            
            // assert
            shellyResult.IsSuccess.Should().BeFalse();
            shellyResult.IsFailure.Should().BeTrue();
            shellyResult.IsTransient.Should().BeTrue();
        }
        
        [Test]
        public async Task Shelly1_WhenResponseNotFound_ShouldReturnSuccess()
        {
            _imposter.AddStub().OnPathEquals("/status").ReturnsStatus(HttpStatusCode.NotFound);
            
            Global.MountebankClient.Submit(_imposter);
            
            Settings settings = Settings.IntegrationTest();

            var sut = new Shelly1Client(settings.User,settings.Password, new Uri(settings.ShellyUri));

            // act
            var shellyResult = await sut.GetStatus(timeout: TimeSpan.FromSeconds(1), cancellationToken: CancellationToken.None);
            
            // assert
            shellyResult.IsSuccess.Should().BeTrue("A not found result is still success since no communication errors occurred");
            shellyResult.Value.Should().BeNull("A not found result should not contain a value");
            shellyResult.IsFailure.Should().BeFalse();
            shellyResult.IsTransient.Should().BeFalse();
        }
        
        [Test]
        public async Task Shelly1_CanRetrieveValidStatus()
        {
            var obj = new
            {
                wifi_sta = new {
                    connected = true,
                    ssid = "abc-home",
                    ip = "10.1.5.10",
                    rssi = -77
                },
                cloud = new
                {
                    connected = true,
                    enabled = true
                },
                mqtt = new
                {
                    connected = true
                },
                update = new
                {
                    status = "pending",
                    has_update = true,
                    new_version = "20200320-123430/v1.6.2@514044b4",
                    old_version = "20200220-123430/v1.6.1@334044b4"
                },
                time = "21:27",
                unixtime = 1639430855,
                serial = 1234,
                uptime = 600,
                ram_total = 35000,
                ram_free = 6000,
                fs_size = 60000,
                fs_free = 5000,
                has_update = true,
                mac = "DC4F2276E575"
            };
            
            _imposter.AddStub().OnPathEquals("/status").ReturnsJson(HttpStatusCode.OK, obj);
            
            Global.MountebankClient.Submit(_imposter);
            
            Settings settings = Settings.IntegrationTest();

            var sut = new Shelly1Client(settings.User,settings.Password, new Uri(settings.ShellyUri));

            // act
            var shellyResult = await sut.GetStatus(timeout: TimeSpan.FromSeconds(1), cancellationToken: CancellationToken.None);

            // assert
            shellyResult.Should().NotBeNull();
            shellyResult.IsSuccess.Should().BeTrue();
            shellyResult.Value.Uptime.Should().BeGreaterThan(0);
            shellyResult.Value.Time.Should().Be("21:27");
            shellyResult.Value.UnixTime.Should().Be(1639430855);
            shellyResult.Value.Serial.Should().Be(1234);
            shellyResult.Value.RamTotal.Should().Be(35000);
            shellyResult.Value.RamFree.Should().Be(6000);
            shellyResult.Value.FileSystemSize.Should().Be(60000);
            shellyResult.Value.FileSystemFree.Should().Be(5000);
            shellyResult.Value.HasUpdate.Should().BeTrue();
            shellyResult.Value.MacAddress.Should().Be("DC4F2276E575");
            shellyResult.Value.WiFiStatus.Connected.Should().BeTrue();
            shellyResult.Value.WiFiStatus.Ip.Should().Be("10.1.5.10");
            shellyResult.Value.WiFiStatus.SSID.Should().Be("abc-home");
            shellyResult.Value.WiFiStatus.RSSI.Should().Be(-77);
            shellyResult.Value.ShellyCloud.Connected.Should().BeTrue();
            shellyResult.Value.ShellyCloud.Enabled.Should().BeTrue();
            shellyResult.Value.Mqtt.Connected.Should().BeTrue();
            shellyResult.Value.Update.Status.Should().Be("pending");
            shellyResult.Value.Update.HasUpdate.Should().BeTrue();
            shellyResult.Value.Update.NewVersion.Should().Be("20200320-123430/v1.6.2@514044b4");
            shellyResult.Value.Update.OldVersion.Should().Be("20200220-123430/v1.6.1@334044b4");
        }
    }
}

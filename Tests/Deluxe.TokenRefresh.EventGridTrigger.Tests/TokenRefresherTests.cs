using AutoFixture;
using Azure.Messaging.EventGrid;
using Deluxe.TokenRefresh.Application;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Deluxe.TokenRefresh.EventGridTrigger.Tests;

[TestClass]
public class TokenRefresherTests
{
    private IDeluxeTokenRefreshService _deluxeTokenRefreshService;
    private ILogger _logger;
    private TokenRefresher _sut;

    [TestInitialize]
    public void Init()
    {
        _deluxeTokenRefreshService = Substitute.For<IDeluxeTokenRefreshService>();
        _logger = Substitute.For<ILogger>();

        _sut = new TokenRefresher(_deluxeTokenRefreshService);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public async Task Run_ThrowsException()
    {
        var data = new { VaultName = "token", ObjectType = "Secret", ObjectName = "newSecret" };
        var eventGridEvent = new EventGridEvent("newSecret", "SecretNewVersionCreated", "1", data);

        _deluxeTokenRefreshService.RefreshToken(Arg.Any<string>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception("There was an error"));

        await _sut.Run(eventGridEvent, _logger, CancellationToken.None);
    }

    [TestMethod]
    public async Task Run_Successfully()
    {
        var data = new { VaultName = "token", ObjectType = "Secret", ObjectName = "newSecret" };
        var eventGridEvent = new EventGridEvent("newSecret", "SecretNewVersionCreated", "1", data);

        await _deluxeTokenRefreshService.RefreshToken(Arg.Any<string>(), Arg.Any<CancellationToken>());

        await _sut.Run(eventGridEvent, _logger, CancellationToken.None);

        _deluxeTokenRefreshService.Received(1);
    }
}
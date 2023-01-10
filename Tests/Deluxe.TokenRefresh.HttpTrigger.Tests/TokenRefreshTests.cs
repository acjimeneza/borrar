using AutoFixture;
using Deluxe.TokenRefresh.Application;
using Deluxe.TokenRefresh.EventGridTrigger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Deluxe.TokenRefresh.HttpTrigger.Tests;

[TestClass]
public class TokenRefreshTests
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
    public async Task Run_ThrowsErrorWhenSecretNameIsNull()
    {
        var request = Substitute.For<HttpRequest>();

        var response = await _sut.Run(request, _logger, CancellationToken.None);

        Assert.AreEqual((response as ObjectResult)?.StatusCode, 500);
        Assert.AreEqual((response as ObjectResult)?.Value.ToString(), "{ error = The secret name parameter is required. }");
    }

    [TestMethod]
    public async Task Run_RefreshTokenThrowsError()
    {
        var values = new Dictionary<string, StringValues>();
        var stringValues = new StringValues("value");
        values.Add("secretName", stringValues);

        var query = new QueryCollection(values);

        var request = Substitute.For<HttpRequest>();
        request.Query = query;

        _deluxeTokenRefreshService.RefreshToken(Arg.Any<string>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception("There was an error."));

        var response = await _sut.Run(request, _logger, CancellationToken.None);

        Assert.AreEqual((response as ObjectResult)?.StatusCode, 500);
        Assert.AreEqual((response as ObjectResult)?.Value.ToString(), "{ error = There was an error. }");
    }

    [TestMethod]
    public async Task Run_Successfully()
    {
        var values = new Dictionary<string, StringValues>();
        var stringValues = new StringValues("value");
        values.Add("secretName", stringValues);

        var query = new QueryCollection(values);

        var request = Substitute.For<HttpRequest>();
        request.Query = query;

        await _deluxeTokenRefreshService.RefreshToken(Arg.Any<string>(), Arg.Any<CancellationToken>());

        var response = await _sut.Run(request, _logger, CancellationToken.None);

        Assert.IsTrue(response is OkObjectResult);
    }
}
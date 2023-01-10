using Deluxe.TokenRefresh.Domain;
using Deluxe.TokenRefresh.Infrastructure.Deluxe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Deluxe.TokenRefresh.Infrastructure.Tests.Deluxe;

[TestClass]
public class ErrorServiceTests
{
    [TestMethod]
    public void BuildError_ReturnsEmptyWhenErrorResponseIsNull()
    {
        var response = ErrorService.BuildError(null);
        Assert.AreEqual(string.Empty, response);
    }

    [TestMethod]
    public void BuildError_ReturnsErrorMessage()
    {
        var errorResponse = new ErrorResponse
        {
            Error = "error message"
        };
        var response = ErrorService.BuildError(errorResponse);
        Assert.AreEqual($"Error: {errorResponse.Error}\r\n", response);
    }

    [TestMethod]
    public void BuildError_ReturnsErrorMessageAndErrors()
    {
        var errorResponse = new ErrorResponse
        {
            Error = "error message",
            Errors = new List<string> {
                "Error 1",
                "Error 2"
            }
        };
        var response = ErrorService.BuildError(errorResponse);
        Assert.AreEqual($"Error: {errorResponse.Error}\r\nErrors:\r\n- {errorResponse.Errors[0]}\r\n- {errorResponse.Errors[1]}\r\n", response);
    }

    [TestMethod]
    public void BuildError_ReturnsCompletedErrorMessage()
    {
        var errorResponse = new ErrorResponse
        {
            Error = "error message",
            Errors = new List<string> {
                "Error 1",
                "Error 2"
            },
            ErrorData = new Dictionary<string, List<string>>
            {
                { "Error Data 1", new List<string> {
                    "ErrorData1Detail1",
                    "ErrorData1Detail2"
                    }
                },
                { "Error Data 2", new List<string> {
                    "ErrorData2Detail1",
                    "ErrorData2Detail2"
                    }
                }
            }
        };
        var response = ErrorService.BuildError(errorResponse);
        Assert.AreEqual("Error: error message\r\nErrors:\r\n- Error 1\r\n- Error 2\r\nErrorData\r\nError Data 1\r\nErrorData1Detail1\r\nErrorData1Detail2\r\nError Data 2\r\nErrorData2Detail1\r\nErrorData2Detail2\r\n", response);
    }
}
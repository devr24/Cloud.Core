using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Cloud.Core.Extensions;
using Cloud.Core.Testing;
using Cloud.Core.Web;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class ApiServiceUnitTest
    {
        private readonly Dictionary<string, string> _headers;

        public ApiServiceUnitTest()
        {
            _headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "Keep-Alive", "true" }
            };
        }

        /// <summary>Check the Api Service is constructred in an expected manor.</summary>
        [Fact]
        public void Test_ApiService_Construction()
        {
            // Empty constuctor testing.
            var apiService = new ApiService();
            apiService.Client.Should().NotBeNull();
            apiService.TryGetAccessToken(out var token).Should().BeFalse();
            token.Should().BeNull();

            // User auth constructor.
            var mockApiService = new Mock<ApiService>("token")
            {
                CallBase = true
            };
            mockApiService.Object.Client.Should().NotBeNull();
            mockApiService.Object.TryGetAccessToken(out token).Should().BeTrue();
            token.Should().Be("token");

            // Factory and auth constuctor.
            apiService = new ApiService(GetHttpClientFactory().Object, new FakeAuth("token"));
            apiService.Client.Should().NotBeNull();
            apiService.TryGetAccessToken(out token).Should().BeTrue();
            token.Should().Be("token");

            // Factory only.
            apiService = new ApiService(GetHttpClientFactory().Object);
            apiService.Client.Should().NotBeNull();
            apiService.TryGetAccessToken(out token).Should().BeFalse();
            token.Should().BeNull();

            // IAuthentication constructor only.
            apiService = new ApiService(new FakeAuth("token"));
            apiService.Client.Should().NotBeNull();
            apiService.TryGetAccessToken(out token).Should().BeTrue();
            token.Should().Be("token");
        }

        /// <summary>Verify the Api Service returns the successful result from a Get request.</summary>
        [Fact]
        public async void Test_ApiService_Get_ReturnsSuccess()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);

            // Act
            var response = await apiService.Get<HttpResponse>("http://contoso.com/getRequestSuccess", _headers);

            // Assert
            response.Message.Should().Be("test");
        }

        /// <summary>Verify Get request returns a bad request response (without body).</summary>
        [Fact]
        public async void Test_ApiService_Get_ReturnsBadReqest_WithoutErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Get<HttpResponse>("http://contoso.com/getRequestFail"));

            // Assert
            exception.Message.Should().Contain("http://contoso.com/getRequestFail");
        }

        /// <summary>Verify Get request returns a bad request response (with body).</summary>
        [Fact]
        public async void Test_ApiService_Get_ReturnsBadReqest_WithErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);

            var errorAction = new Action<HttpResponseMessage>((responseMessage) => {
                string message = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                throw new HttpRequestException($"Bad Time: {message}");
            });

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Get<HttpResponse>("http://contoso.com/getRequestFail", errorAction));

            // Assert
            exception.Message.Should().Contain("Bad Time:");
            exception.Message.Should().Contain("Bad Request Is Bad");
        }

        /// <summary>Verify Post request returns a successful response as expected.</summary>
        [Fact]
        public async void Test_ApiService_Post_ReturnsSuccess()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new StringContent("content", System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await apiService.Post<HttpResponse>("http://contoso.com/getRequestSuccess", _headers, content);

            // Assert
            response.Message.Should().Be("test");
        }

        /// <summary>Verify Post Multipart request returns a successful response as expected.</summary>
        [Fact]
        public async void Test_ApiService_PostMultipart_ReturnsSuccess()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new MultipartFormDataContent
            {
                { new StringContent("content"), "SomeFormValue" }
            };

            // Act
            var response = await apiService.PostMultipart<HttpResponse>("http://contoso.com/getRequestSuccess", content);

            // Assert
            response.Message.Should().Be("test");
        }

        /// <summary>Verify Post request returns a bad request response as expected (without body).</summary>
        [Fact]
        public async void Test_ApiService_Post_ReturnsBadReqest_WithoutErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new StringContent("content", System.Text.Encoding.UTF8, "application/json");

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Post<HttpResponse>("http://contoso.com/getRequestFail", content));
            
            // Assert
            exception.Message.Should().Contain("http://contoso.com/getRequestFail");
        }

        /// <summary>Verify Post request returns a bad request response as expected (with body).</summary>
        [Fact]
        public async void Test_ApiService_Post_ReturnsBadReqest_WithErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new StringContent("content", System.Text.Encoding.UTF8, "application/json");
            var errorAction = new Action<HttpResponseMessage>((responseMessage) => {
                string message = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new HttpRequestException($"Bad Time: {message}");
            });

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Post<HttpResponse>("http://contoso.com/getRequestFail", content, errorAction));
            
            // Assert
            exception.Message.Should().Contain("Bad Time:");
            exception.Message.Should().Contain("Bad Request Is Bad");
        }

        /// <summary>Verify Put request returns a successful response as expected.</summary>
        [Fact]
        public async void Test_ApiService_Put_ReturnsSuccess()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new StringContent("content", System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await apiService.Put<HttpResponse>("http://contoso.com/getRequestSuccess", _headers, content);

            // Assert
            response.Message.Should().Be("test");
        }

        /// <summary>Verify Put request returns a bad request response as expected (without body).</summary>
        [Fact]
        public async void Test_ApiService_Put_ReturnsBadReqest_WithoutErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new StringContent("content", System.Text.Encoding.UTF8, "application/json");

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Put<HttpResponse>("http://contoso.com/getRequestFail", content));
            
            // Assert
            exception.Message.Should().Contain("http://contoso.com/getRequestFail");
        }

        /// <summary>Verify Put request returns a bad request response as expected (with body).</summary>
        [Fact]
        public async void Test_ApiService_Put_ReturnsBadReqest_WithErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var content = new StringContent("content", System.Text.Encoding.UTF8, "application/json");
            var errorAction = new Action<HttpResponseMessage>((responseMessage) => {
                string message = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new HttpRequestException($"Bad Time: {message}");
            });

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Put<HttpResponse>("http://contoso.com/getRequestFail", content, errorAction));
            
            // Assert
            exception.Message.Should().Contain("Bad Time:");
            exception.Message.Should().Contain("Bad Request Is Bad");
        }

        /// <summary>Verify default header of Accept is set.</summary>
        [Fact]
        public async void Test_ApiService_AcceptHeaderCount()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object);

            // Act
            await apiService.Get<HttpResponseMessage>("http://contoso.com/getRequestSuccess");
            
            // Assert
            CountHeaders(apiService).Should().Be(1);
        }

        /// <summary>Verify default Accept header is set but no others.</summary>
        [Fact]
        public async void Test_ApiService_CheckDefaultAcceptHeader()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object);

            // Act
            await apiService.Get<HttpResponseMessage>("http://contoso.com/getRequestSuccess");

            // Assert
            var acceptHeader = apiService.Client.DefaultRequestHeaders.GetHeaderValue("Accept");
            acceptHeader.Should().Be("application/json");

            var doesntExistHeader = apiService.Client.DefaultRequestHeaders.GetHeaderValue("N/A");
            doesntExistHeader.Should().BeNull();
        }

        /// <summary>Verify custom accept headers are set.</summary>
        [Fact]
        public async void Test_ApiService_UseCustomAcceptHeader()
        {
            // Arrange
            var customAcceptHeader = "application/json; odata=verbose";
            var apiService = new ApiService(GetHttpClientFactory().Object);
            var headers = new Dictionary<string, string> {
                { "Accept", customAcceptHeader }
            };

            // Act
            await apiService.Get<HttpResponseMessage>("http://contoso.com/getRequestSuccess", headers);

            // Assert
            CountHeaders(apiService).Should().Be(1);
            var acceptHeader = apiService.Client.DefaultRequestHeaders.GetHeaderValue("Accept");
            acceptHeader.Should().Be(customAcceptHeader);
        }

        /// <summary>Verify Delete request returns a success response as expected.</summary>
        [Fact]
        public async void Test_ApiService_Delete_ReturnsSuccess()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);

            // Act
            var response = await apiService.Delete<HttpResponse>("http://contoso.com/getRequestSuccess", _headers);
            var headersCountBefore = CountHeaders(apiService);

            _headers.Add("Accept-Language", "en-GB");

            response = await apiService.Delete<HttpResponse>("http://contoso.com/getRequestSuccess", _headers);

            var headerCountAfter = CountHeaders(apiService);

            // Assert
            headerCountAfter.Should().Be(headersCountBefore + 1);
            response.Message.Should().Be("test");
        }

        /// <summary>Verify Delete request returns a bad request response as expected (without body).</summary>
        [Fact]
        public async void Test_ApiService_Delete_ReturnsBadReqest_WithoutErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Delete<HttpResponse>("http://contoso.com/getRequestFail"));

            // Assert
            exception.Message.Should().Contain("http://contoso.com/getRequestFail");
        }

        /// <summary>Verify Delete request returns a bad request response as expected (with body).</summary>
        [Fact]
        public async void Test_ApiService_Delete_ReturnsBadReqest_WithErrorAction()
        {
            // Arrange
            var apiService = new ApiService(GetHttpClientFactory().Object, GetAuthentication().Object);
            var errorAction = new Action<HttpResponseMessage>((responseMessage) => {
                string message = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new HttpRequestException($"Bad Time: {message}");
            });

            // Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await apiService.Delete<HttpResponse>("http://contoso.com/getRequestFail", errorAction));
            
            // Assert
            exception.Message.Should().Contain("Bad Time:");
            exception.Message.Should().Contain("Bad Request Is Bad");
        }

        private Mock<IHttpClientFactory> GetHttpClientFactory()
        {
            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected().Setup("Dispose", ItExpr.IsAny<bool>());

            mockHttpMessageHandler.Protected()
                        .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                            ItExpr.Is<HttpRequestMessage>(message => message.RequestUri.ToString().Contains("getRequestSuccess")),
                            ItExpr.IsAny<CancellationToken>()
                        )
                        .ReturnsAsync(new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonConvert.SerializeObject(
                                new HttpResponse
                                {
                                    Message = "test"
                                }
                            )),
                        })
                        .Verifiable();

            mockHttpMessageHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                           ItExpr.Is<HttpRequestMessage>(message => message.RequestUri.ToString().Contains("getRequestFail")),
                           ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.BadRequest,
                           Content = new StringContent(JsonConvert.SerializeObject(
                                new HttpResponse
                                {
                                    Message = "Bad Request Is Bad"
                                }
                            )),
                       })
                       .Verifiable();

            mockClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHttpMessageHandler.Object));
            return mockClientFactory;
        }

        private Mock<IAuthentication> GetAuthentication()
        {
            var accessToken = new Mock<IAccessToken>();
            accessToken.SetupGet(token => token.BearerToken).Returns("token");
            var azureAuthentication = new Mock<IAuthentication>();
            azureAuthentication.SetupGet(x => x.AccessToken).Returns(accessToken.Object);

            return azureAuthentication;
        }

        private int CountHeaders(ApiService apiService)
        {
            var count = 0;
            foreach (var header in apiService.Client.DefaultRequestHeaders)
            {
                count++;
            }
            return count;
        }
    }

    public class HttpResponse
    {
        public string Message { get; set; }
    }

    public class FakeAuth : IAuthentication
    {
        private readonly string _tokenString;

        public FakeAuth(string fakeToken)
        {
            _tokenString = fakeToken;
        }

        public IAccessToken AccessToken => new FakeToken(_tokenString);

        public string Name { get => "example"; set => throw new NotImplementedException(); }
    }

    public class FakeToken : IAccessToken
    {
        private readonly string _tokenString;

        public FakeToken(string fakeToken)
        {
            _tokenString = fakeToken;
        }

        public string BearerToken { get => _tokenString; set => throw new NotImplementedException(); }
        public DateTimeOffset Expires { get => DateTime.Now.AddDays(1); set => throw new NotImplementedException(); }

        public bool HasExpired => false;
    }
}

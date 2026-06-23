using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using System.Text;
using Fundo.Applications.WebApi.Dtos;

namespace Fundo.Services.Tests.Integration
{
    public class LoanManagementControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LoanManagementControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllLoans_ReturnsOkAndLoanList()
        {
            var response = await _client.GetAsync("/api/loans");
            var payload = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Maria Silva", payload);
        }

        [Fact]
        public async Task CreateLoan_ReturnsCreatedLoan()
        {
            var request = new CreateLoanRequest
            {
                Amount = 1000m,
                CurrentBalance = 1000m,
                ApplicantName = "Test User"
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/loans", content);
            var payload = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains("Test User", payload);
        }
    }
}

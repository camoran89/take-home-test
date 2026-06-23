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

        [Fact]
        public async Task PayLoan_ReturnsUpdatedLoan()
        {
            var createRequest = new CreateLoanRequest
            {
                Amount = 500m,
                CurrentBalance = 500m,
                ApplicantName = "Payment Test"
            };
            var createContent = new StringContent(JsonSerializer.Serialize(createRequest), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/loans", createContent);
            createResponse.EnsureSuccessStatusCode();

            var createdPayload = await createResponse.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(createdPayload);
            var loanId = document.RootElement.GetProperty("id").GetInt32();

            var paymentContent = new StringContent(JsonSerializer.Serialize(new { amount = 100m }), Encoding.UTF8, "application/json");
            var paymentResponse = await _client.PostAsync($"/api/loans/{loanId}/payment", paymentContent);
            var payload = await paymentResponse.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, paymentResponse.StatusCode);
            Assert.Contains("Payment Test", payload);
            Assert.Contains("400.00", payload);
        }

        [Fact]
        public async Task GetLoanById_ReturnsLoanDetails()
        {
            var createRequest = new CreateLoanRequest
            {
                Amount = 750m,
                CurrentBalance = 750m,
                ApplicantName = "Detail Test"
            };
            var createContent = new StringContent(JsonSerializer.Serialize(createRequest), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/loans", createContent);
            createResponse.EnsureSuccessStatusCode();

            var createdPayload = await createResponse.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(createdPayload);
            var loanId = document.RootElement.GetProperty("id").GetInt32();

            var response = await _client.GetAsync($"/api/loans/{loanId}");
            var payload = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Detail Test", payload);
            Assert.Contains("750.00", payload);
        }

        [Fact]
        public async Task GetLoanById_ReturnsNotFound_ForMissingLoan()
        {
            var response = await _client.GetAsync("/api/loans/999999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

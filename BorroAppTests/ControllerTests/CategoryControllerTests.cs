using System.Net;
using System.Net.Http.Headers;
using System.Text;

using BorroApp.Data;
using BorroApp.Data.Models;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Xunit.Abstractions;

namespace BorroAppTests.ControllerTests;

public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> {
	private readonly CustomWebApplicationFactory<Program> _factory;
	private readonly ITestOutputHelper                    _testOutputHelper;
	private readonly HttpClient                           _client;
	private          BorroDbContext                       _db;

	public CategoryControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) {
		_factory          = factory;
		_testOutputHelper = testOutputHelper;

		_client = _factory.WithWebHostBuilder(builder => {
			builder.ConfigureServices(services => {
				_db = services.BuildServiceProvider().GetService<BorroDbContext>();
			});
		}).CreateClient();

		_client.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer",
										  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhZGRyZXNzIjoiUmluZ3NodXN2ZWdpZW4gMzVBIiwiaWQiOiIxIiwiZXhwIjoxNzA2MjYzMTY1LCJpc3MiOiJib3Jyby5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6ImJvcnJvLmF6dXJld2Vic2l0ZXMubmV0In0.q9f0TnRqiJL-99gpPJ2FySJk4roC3XXHvkkBvmeXtNg");
	}

	[Fact]
	public async Task TestCreateCategory() {
		// Arrange
		var newCategory = new {
			Type = "Test Category"
		};

		var jsonContent =
			new StringContent(JsonConvert.SerializeObject(newCategory), Encoding.UTF8, "application/json");

		// Act
		var createResponse = await _client.PostAsync("/api/category", jsonContent);

		// Assert
		createResponse.EnsureSuccessStatusCode();
		var createResponseString = await createResponse.Content.ReadAsStringAsync();
		var createdCategory      = JsonConvert.DeserializeObject<Category>(createResponseString);

		Assert.NotNull(createdCategory);
		Assert.Equal(newCategory.Type, createdCategory.Type);
	}
 
	[Fact]
	public async Task TestGetCategory() {
		// Arrange
		var categoryId = 1; // Supposing it's an existing Category Id

		// Act
		var getResponse = await _client.GetAsync($"/api/category/{categoryId}");

		// Assert
		getResponse.EnsureSuccessStatusCode();
		var getResponseString = await getResponse.Content.ReadAsStringAsync();
		var gottenCategory    = JsonConvert.DeserializeObject<Category>(getResponseString);

		Assert.NotNull(gottenCategory);
		Assert.Equal(categoryId, gottenCategory.Id);
	}

	[Fact]
	public async Task TestUpdateCategory() {
		// Arrange
		var updatedCategory = new {
			Id   = 1, // Supposing it's an existing Category Id
			Type = "Updated Category"
		};

		var updatedJsonContent =
			new StringContent(JsonConvert.SerializeObject(updatedCategory), Encoding.UTF8, "application/json");

		// Act
		var putResponse = await _client.PutAsync($"/api/category/{updatedCategory.Id}", updatedJsonContent);

		// Assert
		putResponse.EnsureSuccessStatusCode();
		Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode); // Ensuring the PUT operation returns NoContent

		// Now retrieve the updated resource
		var getResponse = await _client.GetAsync($"/api/category/{updatedCategory.Id}");
		getResponse.EnsureSuccessStatusCode();
		var getResponseString = await getResponse.Content.ReadAsStringAsync();
		var gottenCategory    = JsonConvert.DeserializeObject<Category>(getResponseString);

		Assert.NotNull(gottenCategory);
		Assert.Equal(updatedCategory.Type, gottenCategory.Type);
	}

	[Fact]
	public async Task TestDeleteCategory() {
		// Arrange
		var categoryId = 5; // Supposing it's an existing Category Id

		// Act
		var deleteResponse = await _client.DeleteAsync($"/api/category/{categoryId}");

		// Assert
		deleteResponse.EnsureSuccessStatusCode();
	}
}
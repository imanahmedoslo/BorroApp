using System.Net;
using System.Net.Http.Headers;
using System.Text;

using BorroApp.Data;
using BorroApp.Data.Models;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Xunit.Abstractions;

namespace BorroAppTests.ControllerTests;

public class PostControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> {
	private readonly CustomWebApplicationFactory<Program> _factory;
	private readonly ITestOutputHelper                    _testOutputHelper;
	private readonly HttpClient                           _client;
	private          BorroApp.Data.BorroDbContext         _db;

	public PostControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) {
		_factory               = factory;
		_testOutputHelper = testOutputHelper;

		_client = _factory.WithWebHostBuilder(builder => {
			builder.ConfigureServices(services => {
				_db = services.BuildServiceProvider().GetService<BorroDbContext>();
			});
		}).CreateClient();

		_client.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer",
										  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhZGRyZXNzIjoiTGVpdiBFaXJpa3Nzb25zIGdhdGUgMyIsImlkIjoiMSIsImV4cCI6MTcwNjUzNjE0MywiaXNzIjoiYm9ycm8uYXp1cmV3ZWJzaXRlcy5uZXQiLCJhdWQiOiJib3Jyby5henVyZXdlYnNpdGVzLm5ldCJ9.Dgno4ZmuWtWZfiNJJYpmWbprGa7caW20SK8NC7_OVpY");
	}

	[Fact]
	public async Task TestCreatePost() {
		// Arrange
		var newPost = new {
			Title       = "Test Pbost",
			Image       = "image.jpg",
			Price       = 100.50,
			DateFrom    = DateTime.UtcNow,
			DateTo      = DateTime.UtcNow.AddDays(5),
			Description = "Post description",
			Location    = "Test Location",
			CategoryId  = 1,
			UserId      = 1
		};

		var jsonContent = new StringContent(JsonConvert.SerializeObject(newPost), Encoding.UTF8, "application/json");

		// Act
		var createResponse = await _client.PostAsync("/api/post", jsonContent);

		// Assert
		createResponse.EnsureSuccessStatusCode();
		var createResponseString = await createResponse.Content.ReadAsStringAsync();
		var createdPost          = JsonConvert.DeserializeObject<Post>(createResponseString);

		Assert.NotNull(createdPost);
		Assert.Equal(newPost.Title, createdPost.Title);
	}

	[Fact]
	public async Task TestGetPost() {
		// Arrange
		var postId = 2; // Supposing it's an existing Post Id

		// Act
		var getResponse = await _client.GetAsync($"/api/post/{postId}");

		// Assert
		getResponse.EnsureSuccessStatusCode();
		var getResponseString = await getResponse.Content.ReadAsStringAsync();
		var gottenPost        = JsonConvert.DeserializeObject<Post>(getResponseString);

		Assert.NotNull(gottenPost);
		Assert.Equal(postId, gottenPost.Id);
	}

	[Fact]
	public async Task TestUpdatePost()
	{
		// Arrange
		var updatedPost = new 
		{
			Id          = 23, // Supposing you're using an existing Id
			Title       = "Updated Post",
			Image       = "updated_image.jpg",
			Price       = 150.75,
			DateFrom    = DateTime.UtcNow,
			DateTo      = DateTime.UtcNow.AddDays(7),
			Description = "Updated post description",
			Location    = "Updated location",
			CategoryId  = 1,
			UserId      = 1
		};

		var updatedJsonContent = new StringContent(JsonConvert.SerializeObject(updatedPost), Encoding.UTF8, "application/json");

		// Act
		var putResponse = await _client.PutAsync($"/api/post/{updatedPost.Id}", updatedJsonContent);

		// Assert
		putResponse.EnsureSuccessStatusCode();
		Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode); // Ensuring the PUT operation returns NoContent

		// Now retrieve the updated resource
		var getResponse = await _client.GetAsync($"/api/post/{updatedPost.Id}");
		getResponse.EnsureSuccessStatusCode();
		var getResponseString = await getResponse.Content.ReadAsStringAsync();
		var gottenPost        = JsonConvert.DeserializeObject<Post>(getResponseString);
    
		Assert.NotNull(gottenPost);
		Assert.Equal(updatedPost.Title, gottenPost.Title);
	}

	[Fact]
	public async Task TestDeletePost() {
		// Arrange
		var postId = 29; // Supposing it's an existing Post Id

		// Act
		var deleteResponse = await _client.DeleteAsync($"/api/post/{postId}");

		// Assert
		deleteResponse.EnsureSuccessStatusCode();
	}
}
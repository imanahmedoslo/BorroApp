using System.Net.Http.Headers;
using System.Text;

using BorroApp.Controller.Unauthorized;
using BorroApp.Data.Models;

using Newtonsoft.Json;

namespace BorroAppTests.ControllerTests;

public class UserInfoControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UserInfoControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "<your-token>");
    }

    [Fact]
    public async Task TestGetUserInfos()
    {
        var response = await _client.GetAsync("/api/userinfo");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var userInfos = JsonConvert.DeserializeObject<List<UserInfo>>(responseString);
        Assert.NotNull(userInfos);
    }

    [Fact]
    public async Task TestGetUserInfo()
    {
        var userId = 1; // replace with actual user Id
        var response = await _client.GetAsync($"/api/userinfo/{userId}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var userInfo = JsonConvert.DeserializeObject<UserInfo>(responseString);
        Assert.NotNull(userInfo);
    }

    [Fact]
    public async Task TestGetUserInfoByPostId() 
    {
        var userId = 1; // replace with actual post Id
        var response = await _client.GetAsync($"/api/userinfo/{userId}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var userContactInfo = JsonConvert.DeserializeObject<UserContactInfo>(responseString);
        Assert.NotNull(userContactInfo);
    }

    [Fact]
    public async Task TestCreateUserInfo()
    {
        var newUserInfo = new
        {
            FirstName   = "Updated TestFirstName",
            LastName    = "Updated TestLastName",
            Address     = "Updated Address",
            PostCode    = 12345,
            City        = "Updated City",
            PhoneNumber = "123456789",
            BirthDate   = DateTime.Now.AddDays(-20),
            About       = "Updated About",
            UserId      = 1 // ensure this UserId exists in your database
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(newUserInfo), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/userinfo", jsonContent);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestUpdateUserInfo()
    {
        var updateUserInfo = new
        {
            Id          = 1, // ensure this Id exists in your database
            FirstName   = "Updated TestFirstName",
            LastName    = "Updated TestLastName",
            Address     = "Updated Address",
            PostCode    = 12345,
            City        = "Updated City",
            PhoneNumber = "123456789",
            BirthDate   = DateTime.Now.AddDays(-20),
            About       = "Updated About",
            UserId      = 1 // ensure this UserId exists in your database
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(updateUserInfo), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/userinfo/{updateUserInfo.Id}", jsonContent);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestDeleteUserInfo()
    {
        var userInfoIdToDelete = 1; // replace with actual ID
        var response           = await _client.DeleteAsync($"/api/userinfo/{userInfoIdToDelete}");
        response.EnsureSuccessStatusCode();
    }
}
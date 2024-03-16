using System.Net.Http.Headers;
using System.Text;

using BorroApp.Data.Models;

using Newtonsoft.Json;

using Xunit.Abstractions;

namespace BorroAppTests.ControllerTests;

public class ReservationControllerTests : IClassFixture<CustomWebApplicationFactory<Program>> 
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public ReservationControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "<your-token>");
    }

    [Fact]
    public async Task TestGetReservations()
    {
        var response = await _client.GetAsync("/api/reservation");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var reservations = JsonConvert.DeserializeObject<List<Reservation>>(responseString);
        Assert.NotNull(reservations);
    }

    [Fact]
    public async Task TestGetReservation()
    {
        var reservationId = 1; // replace with actual reservation Id
        var response = await _client.GetAsync($"/api/reservation/{reservationId}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var reservation = JsonConvert.DeserializeObject<Reservation>(responseString);
        Assert.NotNull(reservation);
    }

    [Fact]
    public async Task TestGetAvailability() 
    {
        var postId = 1; // replace with actual post Id
        var response = await _client.GetAsync($"/api/reservation/availability/{postId}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        // assuming that the response is a list of reservedDates
        var reservedDates = JsonConvert.DeserializeObject<List<object>>(responseString);
        Assert.NotNull(reservedDates);
    }

    [Fact]
    public async Task TestCreateReservation()
    {
        var newReservation = new
        {
            // fill these properties with actual data.
            // The data you fill here should be valid in your database context.
            // For example, PostId should correspond to a real Post in your database.
            DateFrom = DateTime.Now,
            DateTo = DateTime.Now,
            Status = 0, 
            Price = 200.0,
            UserId = 1,
            PostId = 1
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(newReservation), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/reservation", jsonContent);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestUpdateReservation()
    {
        var updateReservation = new
        {
            Id = 1, // ensure this Id exists in your database
            DateFrom = DateTime.Now,
            DateTo = DateTime.Now.AddDays(3),
            Status = 0, 
            Price = 350.0,
            UserId = 1,
            PostId = 1
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(updateReservation), Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/reservation/{updateReservation.Id}", jsonContent);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestDeleteReservation()
    {
        var reservationIdToDelete = 1; // replace with actual ID
        var response = await _client.DeleteAsync($"/api/reservation/{reservationIdToDelete}");
        response.EnsureSuccessStatusCode();
    }
}
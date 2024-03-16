using System.Net.Http.Json;

using BorroApp.Controller.Unauthorized;
using BorroApp.Data.Models;

using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

namespace BorroAppTests;

public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<Program>> {
	private readonly CustomWebApplicationFactory<Program> _factory;

	private string validToken =
		" eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhZGRyZXNzIjoiUmluZ3NodXN2ZWllbiAzNUEiLCJpZCI6IjEiLCJleHAiOjE3MDYyNjMxNjUsImlzcyI6ImJvcnJvLmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjoiYm9ycm8uYXp1cmV3ZWJzaXRlcy5uZXQifQ.q9f0TnRqiJL-99gpPJ2FySJk4roC3XXHvkkBvmeXtNg";

	public UnitTest1(CustomWebApplicationFactory<Program> factory) {
		_factory = factory;
	}


}
using BorroApp.Data;
using BorroApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorroApp.Controller.Unauthorized;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase {
	private readonly BorroDbContext _context;

	public ReservationController(BorroDbContext context) {
		_context = context;
	}

    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        return Ok(await _context.Reservation.ToListAsync());
    }

    [HttpGet("{id:int}")]
	public async Task<IActionResult> GetReservation(int id) {
		var reservation = await _context.Reservation.FindAsync(id);
		if (reservation == null) {
			return NotFound();
		}

		return Ok(reservation);
	}

    [HttpGet("availability/{postId:int}")]
    public async Task<IActionResult> GetAvailability(int postId)
    {
        var reservations = await _context.Reservation
            .Where(r => r.PostId == postId && r.Status == Status.Reserved)
            .ToListAsync();

        var reservedDates = reservations.Select(r => new { r.DateFrom, r.DateTo }).ToList();
        return Ok(reservedDates);
    }

    [Authorize]
    [HttpPost]
	public async Task<IActionResult> CreateReservation(ReservationObject createReservation) {
		Reservation newReservation = new() {
			DateFrom = createReservation.DateFrom,
			DateTo   = createReservation.DateTo,
			Status   = createReservation.Status,
			Price    = createReservation.Price,
			UserId   = createReservation.UserId,
			PostId   = createReservation.PostId
		};

		_context.Reservation.Add(newReservation);
		await _context.SaveChangesAsync();

		return CreatedAtRoute(new { id = newReservation.Id }, newReservation);
	}
    [Authorize]
    [HttpPut("{id:int}")]
	public async Task<IActionResult> UpdateReservation(int id, ReservationObject updateReservation) {
		var reservation = await _context.Reservation.FindAsync(id);
		if (reservation == null) {
			return NotFound();
		}

		reservation.DateFrom = updateReservation.DateFrom;
		reservation.DateTo   = updateReservation.DateTo;
		reservation.Status   = updateReservation.Status;
		reservation.Price    = updateReservation.Price;
		reservation.UserId   = updateReservation.UserId;
		reservation.PostId   = updateReservation.PostId;
		await _context.SaveChangesAsync();

		return NoContent();
	}
    [Authorize]
    [HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteReservation(int id) {
		var reservation = await _context.Reservation.FindAsync(id);
		if (reservation == null) {
			return NotFound();
		}

		_context.Reservation.Remove(reservation);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}

public class ReservationObject {
	public DateTime DateFrom { get; set; }
	public DateTime DateTo   { get; set; }
	public Status   Status   { get; set; }
	public double?   Price    { get; set; }
	public int      UserId   { get; set; }
	public int      PostId   { get; set; }
}
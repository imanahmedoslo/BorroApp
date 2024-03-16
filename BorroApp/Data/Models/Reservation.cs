using System.ComponentModel.DataAnnotations.Schema;

namespace BorroApp.Data.Models;

public class Reservation {
	public int       Id       { get; set; }
	public DateTime DateFrom { get; set; } = DateTime.Now;
	public DateTime DateTo   { get; set; } = DateTime.Now;
	public Status   Status   { get; set; }
	public double?   Price    { get; set; }
	public int      UserId   { get; set; }
	public User?     User     { get; set; }
	public int      PostId   { get; set; }
	public Post?     Post     { get; set; }

	[ForeignKey("Rating")]
	public int? RatingLenderId { get; set; }

	public Rating? RatingLender { get; set; }

	[ForeignKey("Rating")]
	public int? RatingBorrowerId { get; set; }

	public Rating? RatingBorrower { get; set; }
}
namespace BorroApp.Data.Models;

public class User {
	public int                       Id           { get; set; }
	public string                    Email        { get; set; } = string.Empty;
	public string                    Password     { get; set; } = string.Empty;
	public ICollection<Reservation>? Reservations { get; set; }
	public ICollection<Post>?        Posts        { get; set; }
	public UserInfo?                         UserInfo            { get; set; }
}
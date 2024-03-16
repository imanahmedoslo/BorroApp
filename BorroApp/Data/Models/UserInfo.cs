namespace BorroApp.Data.Models;

public class UserInfo {
	public int       Id           { get; set; }
	public string?   FirstName    { get; set; }
	public string?   LastName     { get; set; }
	public string?   ProfileImage { get; set; }
	public string?   Address      { get; set; }
	public int?      PostCode     { get; set; }
	public string?   City         { get; set; }
	public string?   PhoneNumber  { get; set; }
	public DateTime? BirthDate    { get; set; }
	public string?   About        { get; set; }
	public int      UserId       { get; set; }
	public User?     User         { get; set; }
}
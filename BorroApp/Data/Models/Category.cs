namespace BorroApp.Data.Models;

public class Category {
	public int                Id    { get; set; }
	public string            Type  { get; set; } = string.Empty;
	public ICollection<Post>? Posts { get; set; }
}
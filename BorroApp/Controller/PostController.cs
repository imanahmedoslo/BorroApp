using BorroApp.Data.Models;
using BorroApp.Data;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BorroApp.Controller.Unauthorized;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase {
	private readonly BorroDbContext _context;

	public PostController(BorroDbContext context) {
		_context = context;
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetPost(int id) {
		var post = await _context.Post.FindAsync(id);
		if (post == null) {
			return NotFound();
		}

		return Ok(post);
	}

	[HttpGet]
	public async Task<IActionResult> GetPosts() {
		return Ok(await _context.Post.ToListAsync());
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreatePost(PostObject createPost) {
		Post newPost = new Post {
			Title       = createPost.Title,
			Image       = createPost.Image,
			Price       = createPost.Price,
			DateFrom    = createPost.DateFrom,
			DateTo      = createPost.DateTo,
			Description = createPost.Description,
			Location    = createPost.Location,
			CategoryId  = createPost.CategoryId,
			UserId      = createPost.UserId,
		};

		_context.Post.Add(newPost);
		await _context.SaveChangesAsync();

		return CreatedAtRoute(new { id = newPost.Id }, newPost);
	}

	[Authorize]
	[HttpPut("{id:int}")]
	public async Task<IActionResult> UpdatePost(int id, PostObject updatePost) {
		var post = await _context.Post.FindAsync(id);
		if (post == null) {
			return NotFound();
		}

		post.Title       = updatePost.Title;
		post.Image       = updatePost.Image;
		post.Price       = updatePost.Price;
		post.DateFrom    = updatePost.DateFrom;
		post.DateTo      = updatePost.DateTo;
		post.Description = updatePost.Description;
		post.Location    = updatePost.Location;
		post.CategoryId  = updatePost.CategoryId;
		post.UserId      = updatePost.UserId;

		await _context.SaveChangesAsync();

		return NoContent();
	}

	[Authorize]
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeletePost(int id) {
		var post = await _context.Post.FindAsync(id);
		if (post == null) {
			return NotFound();
		}

		_context.Post.Remove(post);
		await _context.SaveChangesAsync();

		return NoContent();
	}

    [Authorize]
	[HttpGet("posts/{id}")]
	public async Task<IActionResult> GetAllUserPosts(int userId)
	{
		ICollection<Post> posts = await _context.Post.Where(p => p.UserId == userId).ToListAsync();
		if (posts == null)
		{
			return NotFound();
		}
		return Ok(posts);

	}
}

public class PostObject {
	public string?  Title       { get; set; }
	public string?  Image       { get; set; }
	public double?  Price       { get; set; }
	public DateTime DateFrom    { get; set; }
	public DateTime DateTo      { get; set; }
	public string   Description { get; set; }
	public string   Location    { get; set; }
	public int      CategoryId  { get; set; }
	public int      UserId      { get; set; }
}
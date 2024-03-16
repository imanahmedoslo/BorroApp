using BorroApp.Data;
using BorroApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BorroApp.Controller.Unauthorized;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase {
	private readonly BorroDbContext _context;

	public CategoryController(BorroDbContext context) {
		_context = context;
	}

	[HttpGet]
	public async Task<IActionResult> GetCategories() {
		return Ok(await _context.Category.ToListAsync());
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetCategory(int id) {
		var category = await _context.Category.FindAsync(id);
		if (category == null) {
			return NotFound();
		}
		return Ok(category);
	}

	[HttpPost]
	public async Task<IActionResult> CreateCategory(CategoryObject categoryObject) {
		Category newCategory = new() {
			Type = categoryObject.Type,
		};

		_context.Category.Add(newCategory);
		await _context.SaveChangesAsync();

		return CreatedAtRoute(new { id = newCategory.Id }, newCategory);
	}
    [Authorize]
    [HttpPut("{id:int}")]
	public async Task<IActionResult> UpdateCategory(int id, CategoryObject title) {
		var category = await _context.Category.FindAsync(id);//(title.Id)
		if (category == null) {
			return NotFound();
		}
        /*
		 * var post= await _context.Post.FindAsync(id);
		 * post. CategoryId=title.Id
		 * await _context.SaveChangesAsync();
		 */
        category.Type = title.Type;
		await _context.SaveChangesAsync();

		return NoContent();
	}
    
    [HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteCategory(int id ) {
		var category = await _context.Category.FindAsync(id);
		if (category == null) {
			return NotFound();
		}

		_context.Category.Remove(category);
		await _context.SaveChangesAsync();

		return NoContent();
	}
   // [Authorize]
    /*[HttpDelete("category/{id:int}")]
     public async Task<IActionResult> DeleteCategoryFromPost(int id /*CategoryObject category*///)
    /*{
        var category = await _context.Category.FindAsync(category.Id)
        if (category == null)
        {
            return NotFound();
        }
	var post= await _context.Post.FindAsync(int id);
	 _context.Post.CategoryId=0;
        await _context.SaveChangesAsync();

        return NoContent();
    }*/
}

public class CategoryObject {
	public string? Type { get; set; }
	//public int? CategoryId {get;set;}
}


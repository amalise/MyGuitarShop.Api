using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;

namespace MyGuitarShop.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController(
		ILogger<ProductsController> logger,
		ProductRepository repo)
		: ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			try
			{
				var products = await repo.getAllProductsAsync();
				return Ok(products.Select(p => p.ProductName));
			}
			catch (Exception e)
			{
				logger.LogError(e, "Error fetching Products");

				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}

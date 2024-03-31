using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebMapApi.Models;
using Newtonsoft;

namespace WebMapApi.Controllers
{
	[ApiController]
	[Route("[controller]")]

	public class WebMapController : ControllerBase
	{
		private readonly string pointsFilePath = "points.json";

		[HttpPost("/save-point")]
		public IActionResult SavePoint(PointModel point)
		{
			try
			{
				var points = new List<PointModel>();
				if (System.IO.File.Exists(pointsFilePath))
				{
					points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PointModel>>(System.IO.File.ReadAllText(pointsFilePath));
				}

				point.Id = points.Count > 0 ? points.Max(p => p.Id) + 1 : 0; 
				points.Add(point);

				System.IO.File.WriteAllText(pointsFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(points, Newtonsoft.Json.Formatting.Indented));

				return Ok(new { message = "Point saved successfully" });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
			}
		}

		[HttpGet("/points")]
		public IActionResult GetPoints()
		{
			try
			{
				var points = new List<PointModel>();
				if (System.IO.File.Exists(pointsFilePath))
				{
					points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PointModel>>(System.IO.File.ReadAllText(pointsFilePath));
				}
				return Ok(points);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
			}
		}

		[HttpDelete("/delete-point/{id}")]
		public IActionResult DeletePoint(int id)
		{
			try
			{
				var points = new List<PointModel>();
				if (System.IO.File.Exists(pointsFilePath))
				{
					points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PointModel>>(System.IO.File.ReadAllText(pointsFilePath));
				}

				var pointToRemove = points.FirstOrDefault(p => p.Id == id);
				if (pointToRemove != null)
				{
					points.Remove(pointToRemove);
					System.IO.File.WriteAllText(pointsFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(points, Newtonsoft.Json.Formatting.Indented));
					return Ok(new { message = "Point deleted successfully" });
				}
				else
				{
					return NotFound(new { error = "Point not found" });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal Server Error" });
			}
		}
	}
}

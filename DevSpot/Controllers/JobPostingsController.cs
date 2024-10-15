using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{
	public class JobPostingsController : Controller
	{
		private readonly IRepository<JobPosting> _repository;
		private readonly UserManager<IdentityUser> _userManager;

		public JobPostingsController(
			IRepository<JobPosting> repositor,
			UserManager<IdentityUser> userManager)
		{
			_repository = repositor;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}

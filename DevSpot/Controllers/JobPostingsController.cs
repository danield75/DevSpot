using DevSpot.Constants;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{
	[Authorize]
	public class JobPostingsController : Controller
	{
		private readonly IRepository<JobPosting> _repository;
		private readonly UserManager<IdentityUser> _userManager;

		public JobPostingsController(
			IRepository<JobPosting> repository,
			UserManager<IdentityUser> userManager)
		{
			_repository = repository;
			_userManager = userManager;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var jobPostings = await _repository.GetAllAsync();

			if (User.IsInRole(Roles.EMPLOYER))
			{
				var userId = _userManager.GetUserId(User);
				jobPostings = jobPostings.Where(jp => jp.UserId == userId);
			}

			return View(jobPostings);
		}

		[Authorize(Roles = "Admin, Employer")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin, Employer")]
		public async Task<IActionResult> Create(JobPostingViewModel jobPostingVM)
		{
			if(ModelState.IsValid)
			{
				var jobPosting = new JobPosting
				{
					Title = jobPostingVM.Title,
					Description = jobPostingVM.Description,
					Company = jobPostingVM.Company,
					Location = jobPostingVM.Location,
					UserId = _userManager.GetUserId(User)
				};

				await _repository.AddAsync(jobPosting);

				return RedirectToAction(nameof(Index));
			}

			return View(jobPostingVM);
		}

		[HttpDelete]
		[Authorize(Roles = "Admin, Employer")]
		public async Task<IActionResult> Delete(int id)
		{
			var jobPosting = await _repository.GetByIdAsync(id);
			if (jobPosting == null) 
			{
				return NotFound();
			}

			var userId = _userManager.GetUserId(User);

			if(User.IsInRole(Roles.ADMIN) == false && jobPosting.UserId != userId)
			{
				return Forbid();
			}

			await _repository.DeleteAsync(id);

			return Ok();
		}

		[Authorize(Roles = "Admin, Employer")]
		public async Task<IActionResult> DeleteEasy(int id)
		{
			var jobPosting = await _repository.GetByIdAsync(id);
			if (jobPosting == null)
			{
				return NotFound();
			}

			var userId = _userManager.GetUserId(User);

			if (User.IsInRole(Roles.ADMIN) == false && jobPosting.UserId != userId)
			{
				return Forbid();
			}

			await _repository.DeleteAsync(id);

			return RedirectToAction("Index");
		}
	}
}

﻿using DevSpot.Models;
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
			IRepository<JobPosting> repository,
			UserManager<IdentityUser> userManager)
		{
			_repository = repository;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var jobPostings = await _repository.GetAllAsync();
			return View(jobPostings);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(JobPosting jobPosting)
		{
			return RedirectToAction(nameof(Index));
		}
	}
}

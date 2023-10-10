﻿using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StyleController
	{
		private readonly IAllRepositories<Style> repos;
		private ShopDBContext context = new ShopDBContext();
		private DbSet<Style> style;
		public StyleController()
		{
			style = context.Styles;
			AllRepositories<Style> all = new AllRepositories<Style>(context, style);
			repos = all;
		}
		[HttpGet("get-style")]
		public IEnumerable<Style> GetAll()
		{
			return repos.GetAll();
		}

		[HttpGet("find-style")]
		public IEnumerable<Style> GetStyle(string name)
		{
			return repos.GetAll().Where(x => x.Name == name);
		}

		[HttpPost("create-style")]
		public bool CreateStyle(string StyleCode, string name, int status, DateTime DateCreated)
		{
			Style style = new Style();
			style.StyleID = Guid.NewGuid();
			style.StyleCode = StyleCode;
			style.Name = name;
			style.Status = status;
			style.DateCreated = DateCreated;
			return repos.AddItem(style);
		}

		[HttpPut("update-style")]
		public bool UpdateStyle(Guid id, string StyleCode, string name, int status, DateTime DateCreated)
		{
			Style style = repos.GetAll().First(x => x.StyleID == id);
			style.StyleCode = StyleCode;
			style.Name = name;
			style.Status = status;
			style.DateCreated = DateCreated;
			return repos.EditItem(style);
		}

		[HttpDelete("delete-style")]
		public bool DeleteStyle(Guid id)
		{
			Style sp = repos.GetAll().First(x => x.StyleID == id);
			sp.Status = 1;
			return repos.EditItem(sp);
		}
	}
}

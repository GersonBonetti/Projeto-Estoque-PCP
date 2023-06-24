﻿using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Infra.Data.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		private readonly SQLServerContext _context;

		public UserRepository(SQLServerContext context) : base(context) { }
	}
}

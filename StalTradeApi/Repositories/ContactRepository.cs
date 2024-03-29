﻿using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private readonly StalTradeDbContext _context;
        public ContactRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

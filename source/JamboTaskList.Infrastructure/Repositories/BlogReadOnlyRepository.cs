﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JamboTaskList.Infrastructure.Repositories
{
    public class BlogReadOnlyRepository : IBlogReadOnlyRepository
    {
        private readonly OrderingContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public BuyerRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Buyer Add(Buyer buyer)
        {
            if (buyer.IsTransient())
            {
                //TODO: when migrating to ef core 1.1.1 change Add by AddAsync-. A bug in ef core 1.1.0 does not allow to do it https://github.com/aspnet/EntityFramework/issues/7298 
                return _context.Buyers
                    .Add(buyer)
                    .Entity;
            }
            else
            {
                return buyer;
            }

        }

        public Buyer Update(Buyer buyer)
        {
            return _context.Buyers
                    .Update(buyer)
                    .Entity;
        }

        public async Task<Buyer> FindAsync(string identity)
        {
            var buyer = await _context.Buyers
                .Include(b => b.PaymentMethods)
                .Where(b => b.IdentityGuid == identity)
                .SingleOrDefaultAsync();

            return buyer;
        }
    }
}
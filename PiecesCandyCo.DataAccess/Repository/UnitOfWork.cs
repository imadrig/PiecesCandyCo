﻿using PiecesCandyCo.DataAccess.Data;
using PiecesCandyCo.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiecesCandyCo.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }   
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICustomerOrderDetailRepository CustomerOrderDetail { get; private set; }
        public ICartOrderDetailRepository CartOrderDetail { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            CustomerOrderDetail = new CustomerOrderDetailRepository(_db);
            CartOrderDetail = new CartOrderDetailRepository(_db);
        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

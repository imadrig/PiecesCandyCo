﻿using PiecesCandyCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiecesCandyCo.DataAccess.Repository.IRepository
{
    public interface ICustomerOrderDetailRepository : IRepository<CustomerOrderDetail>
    {
        void Update(CustomerOrderDetail customerOrderDetail);
        void UpdateStatus (int id, string orderStatus, string? paymentStatus = null);
        
    }
}

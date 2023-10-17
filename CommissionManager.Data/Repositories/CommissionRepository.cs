using CommissionManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Repositories
{
    public class CommissionRepository : ICommissionRepository
    {
        public readonly AppDbContext _context;

        public CommissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public Commission GetCommissionById(int commissionId)
        {
            return _context.Commissions.FirstOrDefault(c => c.CommissionId == commissionId);
        }

        public List<Commission> GetAllCommissions()
        {
            return _context.Commissions.ToList();
        }

        public void AddCommission(Commission commission)
        {
            _context.Commissions.Add(commission);
            _context.SaveChanges();
        }

        public void UpdateCommission(Commission commission)
        {
            _context.Entry(commission).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCommission(int commissionId)
        {
            var commissionToDelete = _context.Commissions.Find(commissionId);
            if (commissionToDelete != null)
            {
                _context.Commissions.Remove(commissionToDelete);
                _context.SaveChanges();
            }
        }
    }
}

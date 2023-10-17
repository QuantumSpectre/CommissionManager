using CommissionManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionManager.Data.Repositories
{
    public interface ICommissionRepository
    {
        Commission GetCommissionById(int commissionId);
        List<Commission> GetAllCommissions();
        void AddCommission(Commission commission);
        void UpdateCommission(Commission commission);
        void DeleteCommission(int commissionId);
    }
}

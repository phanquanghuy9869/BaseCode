using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.DataAccess.Repo.EFRepos.Kpi
{
    public interface ICriterionTypeCatalogRepository
    {

    }

    public class CriterionTypeCatalogRepository : ICriterionTypeCatalogRepository, IDisposable
    {
        private readonly IGenericRepository<Kpi_CriterionTypeCatalog> _criterionTypeCatalogRepo;
        private readonly IUnitOfWork _uow;

        public CriterionTypeCatalogRepository(IUnitOfWork unitOfWork)
        {
            this._uow = unitOfWork;
            this._criterionTypeCatalogRepo = this._uow.GetDataRepository<Kpi_CriterionTypeCatalog>();
        }
        
        public void Dispose()
        {
            if (this._uow != null)
            {
                this._uow.Dispose();
            }
        }
    }
}

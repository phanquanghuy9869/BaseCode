using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.BaseServices
{
    public interface IBaseDataService
    {
    }

    public class BaseDataService<TRepository> : IBaseDataService, IDisposable
    {
        protected readonly TRepository _repo;
        public BaseDataService(TRepository repo)
        {
            _repo = repo;
        }

        public void Dispose()
        {
            if (_repo != null && _repo is IDisposable)
            {
                (_repo as IDisposable).Dispose();
            }
        }
    }
}

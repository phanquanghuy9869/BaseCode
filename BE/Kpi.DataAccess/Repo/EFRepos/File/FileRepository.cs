using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kpi.DataAccess.Repo.EFRepos.File
{
    public interface IFileRepository
    {
        IEnumerable<FileDTO> AddRange(IEnumerable<FileDTO> files);
        string GetFilePath(int id);
    }

    public class FileRepository : BaseDataRepository<Kpi_File>, IFileRepository
    {
        private readonly IGenericRepository<Kpi_File> _fileGRepo;

        public FileRepository(IUnitOfWork uow) : base(uow)
        {
            this._fileGRepo = this._unitOfWork.GetDataRepository<Kpi_File>();
        }

        public IEnumerable<FileDTO> AddRange(IEnumerable<FileDTO> files)
        {
            var fileEntity = AutoMapperHelper.Map<FileDTO, Kpi_File, IEnumerable<FileDTO>, IEnumerable<Kpi_File>>(files);
            var rs = this._fileGRepo.AddRange(fileEntity);
            this._unitOfWork.SaveChanges();
            return AutoMapperHelper.Map<Kpi_File, FileDTO, IEnumerable<Kpi_File>, IEnumerable<FileDTO>>(rs);
        }

        public string GetFilePath(int id)
        {
            return _fileGRepo.SelectWhere(x => x.Id == id).Select(x => x.FilePath).FirstOrDefault();
        }
    }
}

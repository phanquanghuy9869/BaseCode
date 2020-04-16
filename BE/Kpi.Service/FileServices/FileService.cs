using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.DataAccess.Repo.EFRepos.File;
using Kpi.Service.BaseServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kpi.Service.FileServices
{
    public interface IFileService
    {
        string GetFilePath(int id);
        FileDataDTO GetFileData(string path);
        IEnumerable<FileDTO> SaveFile(HttpFileCollection files, string rootPath);
    }

    public class FileService : BaseDataService<IFileRepository>, IFileService
    {
        public FileService(IFileRepository repo) : base(repo)
        {
        }

        public string GetFilePath(int id)
        {
            return _repo.GetFilePath(id);
        }      
        
        public FileDataDTO GetFileData(string path)
        {
            string fileName = Path.GetFileName(path);
            string ext = Path.GetExtension(path);
            var stream = File.OpenRead(path);
            string mimeType = MimeTypeHelper.GetMimeType(ext);
            return new FileDataDTO { FileStream = stream, Extension = ext, FileName = fileName, MimeType = mimeType };
        }

        public IEnumerable<FileDTO> SaveFile(HttpFileCollection files, string rootPath)
        {
            var fileList = new List<FileDTO>();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var now = DateTime.Now;
                //var filePath = string.Format(@"{0}\{1)\{2}\{3}\{4}\{5}", rootPath, now.Year, now.Month, now.Day, UniqueIdentityHelper.GetRandomNumber(), file.FileName);
                var folderPath = rootPath + @"\" + now.Year + @"\" + now.Month + @"\" + now.Day + @"\" + UniqueIdentityHelper.GetRandomNumber() + @"\";
                var filePath = folderPath + file.FileName;

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                file.SaveAs(filePath);
                fileList.Add(new FileDTO
                {
                    FileName = file.FileName,
                    FileSize = file.ContentLength,
                    FileType = file.ContentType,
                    FilePath = filePath.Replace(rootPath, string.Empty),
                });
            }

            //save to database
            return this._repo.AddRange(fileList);
        }

    }
}

using Kpi.Api.Configs;
using Kpi.Api.Controllers.Base;
using Kpi.Service.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Kpi.Api.Controllers.File
{
    [Authorize]
    public class FileController : BaseApiController<IFileService>
    {
        public FileController(IFileService service) : base(service)
        {
        }

        [HttpGet]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public HttpResponseMessage Get(int id)
        {
            var filePath = AppConfigs.AssetPath + _service.GetFilePath(id);

            if (string.IsNullOrEmpty(filePath))
            {
                return Request.CreateResponse(HttpStatusCode.OK, Fail("Không tìm thấy file"));
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            var fileData = this._service.GetFileData(filePath);
            response.Content = new StreamContent(fileData.FileStream);            
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(fileData.MimeType);            
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(System.Net.Mime.DispositionTypeNames.Inline)
            {
                FileName = fileData.FileName
            };

            return response;
        }

        [HttpPost]
        public HttpResponseMessage Upload()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var result = _service.SaveFile(httpRequest.Files, AppConfigs.AssetPath);
                return Request.CreateResponse(HttpStatusCode.Created, Success(result));
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
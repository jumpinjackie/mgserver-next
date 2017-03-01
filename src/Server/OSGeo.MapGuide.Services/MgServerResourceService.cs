using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using System.IO;
using Google.Protobuf;

namespace OSGeo.MapGuide.Services
{
    public class MgServerResourceService : MgResourceService.MgResourceServiceBase
    {
        readonly string _contentBasePath;
        readonly string _dataBasePath;

        public MgServerResourceService(string contentBasePath, string dataBasePath)
        {
            _contentBasePath = contentBasePath;
            _dataBasePath = dataBasePath;
        }

        private string GetContentPath(ResourceIdentifier resId)
        {
            return Path.GetFullPath(Path.Combine(_contentBasePath, resId.Path, $"{resId.Name}.{resId.ResourceType}"));
        }

        private string GetDataPath(ResourceIdentifier resId, string dataName)
        {
            return Path.GetFullPath(Path.Combine(_dataBasePath, resId.Path, dataName));
        }

        public override async Task<GetResourceContentResponse> GetResourceContent(GetResourceContentRequest request, ServerCallContext context)
        {
            var response = new GetResourceContentResponse();
            try
            {
                var path = GetContentPath(request.Resource);
                using (var fs = File.OpenRead(path))
                {
                    using (var ms = new MemoryStream())
                    {
                        await fs.CopyToAsync(ms);
                        ms.Position = 0L;
                        var bytes = ms.GetBuffer();
                        response.Result = Resource.Parser.ParseFrom(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error = ErrorUtil.ExceptionToError(ex);
                response.Result = null;
            }
            return response;
        }

        public override async Task<BasicResponse> SetResource(SetResourceRequest request, ServerCallContext context)
        {
            var response = new BasicResponse();
            try
            {
                var path = GetContentPath(request.Resource);
                var parentDir = Path.GetDirectoryName(path);
                if (!Directory.Exists(parentDir))
                {
                    Directory.CreateDirectory(parentDir);
                }
                using (var output = File.OpenWrite(path))// new FileStream(path, FileMode.OpenOrCreate))
                {
                    request.Content.WriteTo(output);
                    /*
                    using (var ms = new MemoryStream())
                    {   
                        request.Content.WriteTo(ms);
                        ms.Position = 0L;
                        var bytes = ms.GetBuffer();
                        await output.CopyToAsync(ms);
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                response.Error = ErrorUtil.ExceptionToError(ex);
            }
            return response;
        }

        public override async Task<ResourceExistsResponse> ResourceExists(ResourceExistsRequest request, ServerCallContext context)
        {
            var response = new ResourceExistsResponse();
            try
            {
                var path = GetContentPath(request.Resource);
                response.Result = File.Exists(path);
            }
            catch (Exception ex)
            {
                response.Error = ErrorUtil.ExceptionToError(ex);
            }
            return response;
        }
    }
}

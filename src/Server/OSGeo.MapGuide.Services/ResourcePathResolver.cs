using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.Services
{
    public class ResourcePathResolver
    {
        readonly string _contentBasePath;
        readonly string _dataBasePath;

        public ResourcePathResolver(string contentBasePath, string dataBasePath)
        {
            _contentBasePath = contentBasePath;
            _dataBasePath = dataBasePath;
        }

        public string GetContentPath(ResourceIdentifier resId)
        {
            return Path.GetFullPath(Path.Combine(_contentBasePath, resId.Path, $"{resId.Name}.{resId.ResourceType}"));
        }

        public string GetDataPath(ResourceIdentifier resId, string dataName = null)
        {
            if (string.IsNullOrEmpty(dataName))
                return Path.GetFullPath(Path.Combine(_dataBasePath, resId.Path)) + Path.DirectorySeparatorChar;
            else
                return Path.GetFullPath(Path.Combine(_dataBasePath, resId.Path, dataName));
        }
    }
}

using System;
using System.Linq;

namespace OSGeo.MapGuide
{
    partial class ResourceIdentifier
    {
        /// <summary>
        /// Parses the given resource id string to a <see cref="ResourceIdentifer"/>
        /// </summary>
        /// <param name="resIdStr"></param>
        /// <returns></returns>
        public static ResourceIdentifier Parse(string resIdStr)
        {
            var parts = resIdStr.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
            var subParts = resIdStr.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var resName = subParts[subParts.Length - 1];
            var resParts = resName.Split('.');

            var resId = new ResourceIdentifier
            {
                RepositoryType = parts[0],
                Path = string.Join("/", subParts.Skip(1).Take(subParts.Length - 2)),
                Name = resParts[0],
                ResourceType = resParts[1]
            };
            return resId;
        }
    }
}
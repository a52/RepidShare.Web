using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace RepidShare.Entities
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(Type ResourceFile, string resourceId)
            : base(GetMessageFromResource(ResourceFile, resourceId))
        { }

        private static string GetMessageFromResource(Type ResourceFile, string resourceId)
        {
            System.Resources.ResourceManager obj = new System.Resources.ResourceManager(ResourceFile);
            return obj.GetString(resourceId);
        }
    }
}

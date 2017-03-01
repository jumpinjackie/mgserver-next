using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.Services
{
    public class ErrorUtil
    {
        public static Error ExceptionToError(Exception ex)
        {
            var err = new Error
            {
                Name = ex.GetType().Name,
                Message = ex.Message
            };
            err.Stack.AddRange(ex.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            return err;
        }
    }
}

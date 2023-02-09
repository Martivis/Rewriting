using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Common.Exceptions;

public class ServiceNotFoundException : Exception
{
    public string Message { get; set; }
    public Type ServiceType { get; set; }

    public ServiceNotFoundException(string message, Type serviceType)
    {
        Message = message;
        ServiceType = serviceType;
    }
    public ServiceNotFoundException(Type serviceType)
    {
        Message = $"Service {serviceType} not found";
        ServiceType = serviceType;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.Responses
{
    public record Response(bool Flag = false, string message = null!)
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection_2_BL.Auth
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, string role);
    }
}

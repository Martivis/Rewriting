using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount;

public interface IUserDataService
{
    Task<string> GetUserEmailAsync(Guid userUid);
}

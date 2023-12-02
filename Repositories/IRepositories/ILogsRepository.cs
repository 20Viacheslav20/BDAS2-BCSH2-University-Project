using BDAS2_BCSH2_University_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ILogsRepository
    {
        List<Logs> GetAll();
    }
}

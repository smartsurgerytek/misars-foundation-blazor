using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Misars.Foundation.App.EntityFrameworkCore;

namespace Misars.Foundation.App.Filemanagements
{
    public class EfCoreFilemanagementRepository : EfCoreFilemanagementRepositoryBase, IFilemanagementRepository
    {
        public EfCoreFilemanagementRepository(IDbContextProvider<AppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = Infraestructure.Entity.Entities.Other.Version;

namespace TopMai.Domain.Services.Other.Interfaces
{
    public interface IVersionService
    {
        Task<object> Post(Version version);
        List<Version> GetAll();
        Version Get(Guid id);
        Task<object> Put(Version newVersion);
        Task<object> Delete(Guid id);

        Version GetActualVersion(string platform, bool required);
    }
}

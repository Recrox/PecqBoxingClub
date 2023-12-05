using RamDam.BackEnd.Core.Models.Table;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace RamDam.BackEnd.Core.Services
{
    public class ApplicationSieveService : SieveProcessor
    {
        public ApplicationSieveService(IOptions<SieveOptions> options) : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            return mapper;
        }
    }
}

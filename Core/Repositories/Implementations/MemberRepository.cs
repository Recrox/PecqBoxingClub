
using AutoMapper;
using Core.Repositories.Factory;
using Database;
using Microsoft.EntityFrameworkCore;
using PecqBoxingClubApi.BackEnd.Core.Models.Table;

namespace Core.Repositories.Implementations;
public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(BoxingClubContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}

using AutoMapper;
using Core.Models.Api;
using Core.Repositories;

namespace Core.Services.Implementations;
public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper mapper;

    public MemberService(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<Member>> GetMembersAsync()
    {
        var members =  await _memberRepository.GetAllAsync();
        return members.Select(m => this.mapper.Map<PecqBoxingClubApi.BackEnd.Core.Models.Table.Member, Member > (m));
    }

    //public Member GetMemberById(int id)
    //{
    //    return _memberRepository.GetAsync(id);
    //}

    public async Task AddMember(Member member)
    {
        var memberToAdd = this.mapper.Map<Member, PecqBoxingClubApi.BackEnd.Core.Models.Table.Member>(member);
        await _memberRepository.AddAsync(memberToAdd);
    }

    //public void UpdateMember(Member member)
    //{
    //    _memberRepository.UpdateAsync(member);
    //}

    //public void DeleteMember(int id)
    //{
    //    _memberRepository.DeleteAsync(id);
    //}
}


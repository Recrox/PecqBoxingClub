using Core.Models.Api;

namespace Core.Services;
public interface IMemberService
{
    Task<IEnumerable<Member>> GetMembersAsync();
    //Member GetMemberById(int id);
    //Member AddMember(Member member);
    //void UpdateMember(Member member);
    //void DeleteMember(int id);
}
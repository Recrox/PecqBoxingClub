using Core.Models.Api;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace PecqBoxingClub.Controllers;

[ApiController]
[Route("[controller]/members")]
public class MemberController : ControllerBase
{
    private readonly IMemberService memberService;

    public MemberController(IMemberService memberService)
    {
        this.memberService = memberService;
    }

    [HttpGet]
    public async Task<IEnumerable<Member>> GetMembersAsync()
    {
        return await this.memberService.GetMembersAsync();
    }

    //[HttpGet("{id}")]
    //public ActionResult<Member> GetMemberById(int id)
    //{
    //    var member = this.memberService.GetMemberById(id);
    //    if (member == null)
    //    {
    //        return NotFound();
    //    }
    //    return member;
    //}

    //[HttpPost]
    //public ActionResult<Member> AddMember(Member member)
    //{
    //    this.memberService.AddMember(member);
    //    if (member == null)
    //    {
    //        return BadRequest();
    //    }
    //    return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
    //}

    //[HttpPut("{id}")]
    //public IActionResult UpdateMember(int id, Member member)
    //{
    //    memberService.UpdateMember(id, member);

    //    return Ok();
    //}

    //[HttpDelete("{id}")]
    //public IActionResult DeleteMember(int id)
    //{
    //    this.memberService.DeleteMember(id);

    //    return Ok();
    //}
}

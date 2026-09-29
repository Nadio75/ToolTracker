using System.Reflection.Metadata;

namespace ToolTracker.Models;

public record MemberResponse(Guid Id, string Name, string ContactNumber)
{
    public static MemberResponse FromMember(Member member) => new(member.Id, member.Name, member.Contact);
}
    
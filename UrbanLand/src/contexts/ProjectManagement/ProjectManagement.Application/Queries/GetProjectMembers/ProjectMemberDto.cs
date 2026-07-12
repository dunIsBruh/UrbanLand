using Core.Identity;
using Core.Identity;

namespace ProjectManagement.Application.Queries.GetProjectMembers;

public record ProjectMemberDto(
    UserId UserId,
    string Role,
    DateTime JoinedAt);

using SharedKernel.Identity;

namespace ProjectManagement.Application.Queries.GetProjectMembers;

public record ProjectMemberDto(
    UserId UserId,
    string Role,
    DateTime JoinedAt);

using MediatR;

namespace HMS.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Username, string Email, string Password, string Role) : IRequest<Guid>;
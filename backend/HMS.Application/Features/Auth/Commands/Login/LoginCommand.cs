using MediatR;

namespace HMS.Application.Features.Auth.Commands.Login;

// Trả về một chuỗi string (chính là JWT Access Token)
public record LoginCommand(string Username, string Password) : IRequest<string>;
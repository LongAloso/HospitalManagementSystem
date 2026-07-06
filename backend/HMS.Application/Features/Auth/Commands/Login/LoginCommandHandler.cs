using HMS.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HMS.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Tìm user theo Username
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Sai tên đăng nhập hoặc mật khẩu.");
        }

        // 2. Verify mật khẩu bằng BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Sai tên đăng nhập hoặc mật khẩu.");
        }

        // 3. Nếu hợp lệ, sinh Access Token
        var accessToken = _tokenService.GenerateAccessToken(user);

        return accessToken;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HMS.API.Filters;

// 1. Định nghĩa Attribute để có thể dùng [HasPermission(Permissions.CreatePrescription)]
public class HasPermissionAttribute : TypeFilterAttribute
{
    public HasPermissionAttribute(string permission) : base(typeof(HasPermissionFilter))
    {
        Arguments = new object[] { permission };
    }
}

// 2. Logic kiểm tra của Attribute
public class HasPermissionFilter : IAuthorizationFilter
{
    private readonly string _permission;

    public HasPermissionFilter(string permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Kiểm tra xem User đã đăng nhập chưa
        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Kiểm tra xem trong Claims của Token có chứa Permission yêu cầu không
        var hasPermission = context.HttpContext.User.Claims
            .Any(c => c.Type == "Permission" && c.Value == _permission);

        if (!hasPermission)
        {
            // Trả về 403 Forbidden: Đã đăng nhập nhưng không đủ quyền
            context.Result = new ForbidResult();
        }
    }
}
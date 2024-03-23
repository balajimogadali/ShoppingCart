using Microsoft.AspNetCore.Identity;

namespace ShoppingCartApi.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}

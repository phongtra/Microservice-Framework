namespace Identity.WebApi.Services
{
    public interface IUserService
    {
        Models.SecurityToken Authenticate(string username, string password);
    }
}
using SaasMultiTenant.BL.Interfaces;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;
using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;
using SaasMultiTenant.Utils;

namespace SaasMultiTenant.BL
{
    public class AuthBL: IAuthBL
    {

        private readonly IUserDAL _userDAL;
        private readonly IWorkspaceDAL _workspaceDAL;

        public AuthBL(IUserDAL userDAL, IWorkspaceDAL workspaceDAL)
        {
            _userDAL = userDAL;
            _workspaceDAL = workspaceDAL;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    throw new ArgumentException("El email y la contraseña son requeridos.");
                }

                User? user = await _userDAL.GetUserByEmail(loginRequest.Email);

                if (user == null)
                {
                    throw new ArgumentException("El email y/o la contraseña son incorrectos.");
                }

                bool isPasswordValid = PasswordHasher.VerifyPassword(loginRequest.Password, user.Password);

                if (!isPasswordValid)
                {
                    throw new ArgumentException("El email y/o la contraseña son incorrectos.");
                }

                List<Workspace> workspaces = await _workspaceDAL.GetWorkspacesByUserId(user.Id);

                if (workspaces == null || !workspaces.Any())
                {
                    throw new ArgumentException("El usuario no esta asociado a ningun Workspaces.");
                }

                LoginResponse response = new LoginResponse();

                response.User = user;
                response.Workspaces = workspaces;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

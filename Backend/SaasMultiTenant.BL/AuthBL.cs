using SaasMultiTenant.BL.Interfaces;
using SaasMultiTenant.DAL.Interfaces;
using SaasMultiTenant.Entity.Models;
using SaasMultiTenant.Entity.Request;
using SaasMultiTenant.Entity.Response;
using SaasMultiTenant.Utils;

namespace SaasMultiTenant.BL
{
    public class AuthBL : IAuthBL
    {

        private readonly IUserDAL _userDAL;
        private readonly IWorkspaceDAL _workspaceDAL;
        private readonly ITokenService _tokenService;
        private readonly IRolesDAL _rolesDAL;

        public AuthBL(IUserDAL userDAL, IWorkspaceDAL workspaceDAL, ITokenService tokenService, IRolesDAL rolesDAL)
        {
            _userDAL = userDAL;
            _workspaceDAL = workspaceDAL;
            _tokenService = tokenService;
            _rolesDAL = rolesDAL;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    throw new ArgumentException("El email y la contraseña son requeridos.");
                }

                if (!Validator.IsValidEmail(loginRequest.Email))
                {
                    throw new ArgumentException("El email tiene un formato invalido.");
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


        public async Task<TokenResponse> GenerateToken(TokenRequest tokenRequest)
        {
            try
            {
                User? user = await _userDAL.GetUserById(tokenRequest.UserId);

                if (user == null)
                {
                    throw new ArgumentException("La sesión del usuario no es válida o la cuenta ya no existe.");
                }

                UserWorkspace? userWorkspace = await _workspaceDAL.GetUserWorkspaceByUserIdAndWorkspaceId(tokenRequest.UserId, tokenRequest.WorkspaceId);

                if (userWorkspace == null)
                {
                    throw new ArgumentException("Tu cuenta no tiene asignado ningún espacio de trabajo.");
                }

                Role? role = await _rolesDAL.GetRoleById(userWorkspace.RoleId);

                if (role == null)
                {
                    throw new ArgumentException("El rol especificado para este espacio de trabajo no existe.");
                }

                string username = user.Firstname + " " + user.Lastname;
                string token = _tokenService.GenerateUserToken(username, user.Id, user.Email, role.Name);

                TokenResponse response = new TokenResponse();
                response.Token = token;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

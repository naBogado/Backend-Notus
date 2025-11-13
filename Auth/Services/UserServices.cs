using AutoMapper;
using Notus.Enums;
using Notus.Models.Role;
using Notus.Models.User;
using Notus.Models.User.Dto;
using Notus.Repositories;
using Notus.Utils;
using System.Data;
using System.Net;

namespace Notus.Services
{
    public class UserServices
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IEncoderServices _encoderServices;
        private readonly RoleServices _roleServices;

        public UserServices(IUserRepository repo, IMapper mapper, IEncoderServices encoderServices, RoleServices roleServices)
        {
            _repo = repo;
            _mapper = mapper;
            _encoderServices = encoderServices;
            _roleServices = roleServices;
        }

        async public Task<List<UserWithoutPassDTO>> GetAll()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<List<UserWithoutPassDTO>>(users);
        } 

        async public Task<User> GetOneByEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Email is empty");
            }

            var user = await _repo.GetOneAsync(x => x.Email == email);
            return user;
        }

        async public Task<UserWithoutPassDTO> CreateOne(RegisterDTO register)
        {
            var user = _mapper.Map<User>(register);

            user.Password = _encoderServices.Encode(user.Password);

            var role = await _roleServices.GetOneByName(ROLE.USER);
            user.Roles = new() { role };

            await _repo.CreateOneAsync(user);

            return _mapper.Map<UserWithoutPassDTO>(user);
        }

        async public Task<UserWithoutPassDTO> UpdateOneByEmail(string? email, UpdateUserDTO updateDto)
        {
            var user = await GetOneByEmail(email);

            var userMapped = _mapper.Map(updateDto, user);

            await _repo.UpdateOneAsync(userMapped);

            return _mapper.Map<UserWithoutPassDTO>(userMapped);
        }

        async public Task<UserWithoutPassDTO> AddRoleByEmail(string? email, string? roleName)
        {
            // validaciones
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(roleName))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Email and role are required.");
            }

            // busca usuario
            var user = await _repo.GetOneAsync(x => x.Email == email);
            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, $"User with email: {email} not found.");
            }

            // busca rol
            var role = await _roleServices.GetOneByName(roleName);
            if (role == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, $"Role '{roleName}' not found.");
            }

            // si no existe la lista, la crea (no deberia pasar)
            if (user.Roles == null)
            {
                user.Roles = new List<Role>();
            }

            // evitar roles duplicados
            if (user.Roles.Any(r => r.Name == role.Name))
            {
                return _mapper.Map<UserWithoutPassDTO>(user);
            }

            // añade el rol y lo guarda (persistencia)
            user.Roles.Add(role);
            await _repo.UpdateOneAsync(user);

            return _mapper.Map<UserWithoutPassDTO>(user);
        }

        async public Task<UserWithoutPassDTO> RemoveRoleByEmail(string? email, string? roleName)
        {
            // validaciones
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(roleName))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Email and role are required.");
            }

            // busca usuario
            var user = await _repo.GetOneAsync(x => x.Email == email);
            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, $"User with email: {email} not found.");
            }

            // busca rol
            var role = await _roleServices.GetOneByName(roleName);
            if (role == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, $"Role '{roleName}' not found.");
            }

            // si no existe la lista, no hay nada que eliminar (no deberia pasar)
            if (user.Roles == null || !user.Roles.Any())
            {
                return _mapper.Map<UserWithoutPassDTO>(user);
            }

            // buscar coincidencia (por Id si está, por nombre como fallback) - comparación insensible a mayúsculas
            var roleInUser = user.Roles.FirstOrDefault(r =>
                (r.Id != 0 && r.Id == role.Id) ||
                string.Equals(r.Name, role.Name)
                //compara los nombres de los roles
            );

            // si el usuario no tiene ese rol lo devuelve como está
            if (roleInUser == null)
            {
                return _mapper.Map<UserWithoutPassDTO>(user);
            }

            // elimina el rol y guarda (persistencia)
            user.Roles.Remove(roleInUser);
            await _repo.UpdateOneAsync(user);

            return _mapper.Map<UserWithoutPassDTO>(user);
        }

    }
}

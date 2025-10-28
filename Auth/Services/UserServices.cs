using Notus.Enums;
using Notus.Models.User;
using Notus.Models.User.Dto;
using Notus.Repositories;
using Notus.Utils;
using AutoMapper;
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
    }
}

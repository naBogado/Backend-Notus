using AutoMapper;
using Notus.Models.Class;
using Notus.Models.Class.Dto;
using Notus.Models.User;
using Notus.Models.User.Dto;
using Notus.Repositories;
using System.Net;

namespace Notus.Services
{
    public class ClassServices
    {
        private readonly IMapper _mapper;

        public ClassServices(IMapper mapper)
        {
            _mapper = mapper;
        }

        async public Task<ClassWithNames> CreateOne(Class createClass)
        {
            //var studentsId = await _roleServices.GetUsers(algo);
            //createClass.StudentId = new() { studentsId };

            //await _repo.CreateOneAsync(user);

            return _mapper.Map<ClassWithNames>(createClass);
        }

        //async public Task<Class> UpdateOneById()
        //{

        //}
        //async public Task<Class> DeleteOneById()
        //{

        //}

        //async public Task<Class> GetOneById()
        //{

        //}
        //async public Task<List<Class>> GetAllByProfessor()
        //{

        //}
    }
}

using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _repository.FindById(id);
            return await _repository.Delete(entity);
        }

        public List<UserDTO> FindAll()
        {
            return _repository.FindAll()
                              .Select(c => new UserDTO()
                              {
                                  id = c.Id,
                                  name = c.Name,
								  username = c.Username,
                                  password = c.Password,
                                  roleId = c.RoleId,
                                  role = new Role()
                                  {
                                      Id = c.Role.Id,
                                      Level = c.Role.Level,
                                      LevelId = c.Role.LevelId
                                  }
							  }).ToList();
        }

        public async Task<UserDTO> FindById(int id)
        {
            var dto = new UserDTO();
            return dto.mapToDTO(await _repository.FindById(id));
        }

        public Task<int> Save(UserDTO dto)
        {
            if (dto.id > 0)
            {
                return _repository.Update(dto.mapToEntity());
            }
            else
            {
                return _repository.Save(dto.mapToEntity());
            }
        }

    }
}

using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _repository.FindById(id);
            return await _repository.Delete(entity);
        }

        public List<RoleDTO> FindAll()
        {
            return _repository.FindAll()
                              .Select(c => new RoleDTO()
                              {
                                  id = c.Id,
								  level = c.Level,
                                  levelId = c.LevelId
							  }).ToList();
        }

        public async Task<RoleDTO> FindById(int id)
        {
            var dto = new RoleDTO();
            return dto.mapToDTO(await _repository.FindById(id));
        }

        public Task<int> Save(RoleDTO dto)
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

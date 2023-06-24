using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repository;

        public UnitService(IUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _repository.FindById(id);
            return await _repository.Delete(entity);
        }

        public List<UnitDTO> FindAll()
        {
            return _repository.FindAll()
                              .Select(c => new UnitDTO()
                              {
                                  id = c.Id,
								  name = c.Name
							  }).ToList();
        }

        public async Task<UnitDTO> FindById(int id)
        {
            var dto = new UnitDTO();
            return dto.mapToDTO(await _repository.FindById(id));
        }

        public Task<int> Save(UnitDTO dto)
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

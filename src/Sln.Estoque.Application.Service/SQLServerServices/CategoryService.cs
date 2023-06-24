using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _repository.FindById(id);
            return await _repository.Delete(entity);
        }

        public List<CategoryDTO> FindAll()
        {
            return _repository.FindAll()
                                .Select(c => new CategoryDTO()
                                        {
                                            id = c.Id,
                                            name = c.Name
                                        }).ToList();
        }

        public async Task<CategoryDTO> FindById(int id)
        {
            var dto = new CategoryDTO();
            return dto.mapToDTO(await _repository.FindById(id));
        }

        public Task<int> Save(CategoryDTO dto)
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
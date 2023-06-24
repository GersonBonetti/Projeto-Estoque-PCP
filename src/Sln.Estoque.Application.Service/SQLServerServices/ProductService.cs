using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _repository.FindById(id);
            return await _repository.Delete(entity);
        }

        public List<ProductDTO> FindAll()
        {
            var lista =  _repository.FindAll()
                              .Select(c => new ProductDTO()
                              {
                                  id = c.Id,
                                  codeProduct = c.CodeProduct,
                                  name = c.Name,
                                  alias = c.Alias,
                                  quantity = c.Quantity,
                                  minimumQuantity = c.MinimumQuantity,
                                  unitId = c.UnitId,
                                  price = c.Price,
                                  categoryId = c.CategoryId,
                                  updateTime = c.UpdateTime,
                                  unit = new Unit()
                                  {
                                      Id = c.Unit.Id,
                                      Name = c.Unit.Name
                                  },
                                  category = new Category()
                                  {
                                      Id = c.Category.Id,
                                      Name = c.Category.Name
                                  }
                              }).ToList();
            return lista;
        }

        public async Task<ProductDTO> FindById(int id)
        {
            var dto = new ProductDTO();
            return dto.mapToDTO(await _repository.FindById(id));
        }

        public Task<int> Save(ProductDTO dto)
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

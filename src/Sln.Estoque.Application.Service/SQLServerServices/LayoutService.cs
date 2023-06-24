using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
    public class LayoutService : ILayoutService
    {
        private readonly ILayoutRepository _layoutRepository;

        public LayoutService(ILayoutRepository layoutRepository)
        {
            _layoutRepository = layoutRepository;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _layoutRepository.FindById(id);
            return await _layoutRepository.Delete(entity);
        }

        public List<LayoutDTO> FindAll()
        {
            return _layoutRepository.FindAll()
                                    .Select(l => new LayoutDTO()
                                    {
                                        id = l.Id,
                                        viewName = l.ViewName,
                                        fileName = l.FileName,
                                        multiplier = l.Multiplier,
                                        method = l.Method,
                                        quantityPosition = l.QuantityPosition
                                    }).ToList();
        }

        public async Task<LayoutDTO> FindById(int id)
        {
            var dto = new LayoutDTO();
            return dto.mapToDTO(await _layoutRepository.FindById(id));
        }

        public Task<int> Save(LayoutDTO dto)
        {
            if (dto.id > 0)
            {
                return _layoutRepository.Update(dto.mapToEntity());
            }
            else
            {
                return _layoutRepository.Save(dto.mapToEntity());
            }
        }
    }
}

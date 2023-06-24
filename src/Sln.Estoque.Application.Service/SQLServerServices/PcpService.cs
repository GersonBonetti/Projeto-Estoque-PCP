using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Application.Service.SQLServerServices
{
	public class PcpService : IPcpService
	{
		private readonly IPcpRepository _repository;

        public PcpService(IPcpRepository repository)
        {
			_repository = repository;
		}

        public async Task<int> Delete(int id)
		{
			var entity = await _repository.FindById(id);
			return await _repository.Delete(entity);
		}

		public List<FinishedOrderDTO> FindAll()
		{
			return _repository.FindAll()
							  .Select(o => new FinishedOrderDTO()
							  {
								  id = o.Id,
								  orderId = o.OrderId,
								  layoutCode = o.LayoutCode,
								  quantity = o.Quantity,
								  dateFinish = o.DateFinish,
								  userId = o.UserId,
								  user = new User()
								  {
									  Id = o.User.Id,
									  Name = o.User.Name,
									  Username = o.User.Username,
									  RoleId = o.User.RoleId,
								  }
							  }).ToList();
		}

		public async Task<FinishedOrderDTO> FindById(int id)
		{
			var dto = new FinishedOrderDTO();
			return dto.mapToDTO(await _repository.FindById(id));
		}

		public Task<int> Save(FinishedOrderDTO dto)
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

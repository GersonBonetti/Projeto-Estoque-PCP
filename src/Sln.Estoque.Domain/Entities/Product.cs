namespace Sln.Estoque.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string CodeProduct { get; set; }
        public string Name { get; set; }
        public string? Alias { get; set; }
        public decimal? Quantity { get; set; }
        public int? MinimumQuantity { get; set; }
        public int UnitId { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdateTime { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}

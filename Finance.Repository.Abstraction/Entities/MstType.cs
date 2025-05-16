namespace Finance.Repository.Abstraction.Entities
{
    public class MstType
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = null!;
        public string Description { get; set; } = default!;
    }
}

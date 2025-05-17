namespace Finance.Repository.Abstraction.Entities
{
    public class MstType
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = null!;
        public string? Description { get; set; }
        public bool Enable { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}

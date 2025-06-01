namespace Finance.Shared.Models.UserBalance
{
    public class UserBalanceModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public Guid AppId { get; set; }
        public string AppName { get; set; } = null!;
        public double Balance { get; set; }
    }
}

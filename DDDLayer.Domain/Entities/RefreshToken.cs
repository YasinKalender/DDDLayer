using Dapper.Contrib.Extensions;

namespace DDDLayer.Domain.Entities
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expration { get; set; }
    }
}

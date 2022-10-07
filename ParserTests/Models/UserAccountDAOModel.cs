using System.ComponentModel.DataAnnotations;

namespace ParserTests.Models
{
    public class UserAccountDAOModel
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public string CardType { get; set; } = string.Empty;
    }
}

using DataAccess.Identity;

namespace BusinessLogic.DTOs
{
    public class BankAccountTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static BankAccountTypeDto ToDto(BankAccountType bankAccountType)
        {
            return new BankAccountTypeDto() { Id = bankAccountType.Id, Name = bankAccountType.Name };
        }
    }
}
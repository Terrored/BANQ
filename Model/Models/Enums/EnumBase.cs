namespace Model.Models.Enums
{
    public class EnumBase<TEnum> : BaseEntity where TEnum : struct
    {
        public string Name { get; set; }
    }
}

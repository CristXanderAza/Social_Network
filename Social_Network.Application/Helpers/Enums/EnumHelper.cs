using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Application.Helpers.Enums
{
    public static class EnumHelper
    {
        public static IEnumerable<Ident<int>> GetEnumsAsIdent<TEnum>() where TEnum : Enum
         => Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(en => new Ident<int>
            {
                Id = Convert.ToInt32(en), 
                Name = en.ToString()
            });
    }
}

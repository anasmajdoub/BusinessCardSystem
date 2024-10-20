using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Domain.Shared.Filter.Enums;

namespace BizCardSystem.Domain.Shared.Filter.Configuration
{
    public static class DefaultFilterValue
    {
        public static int PageIndex { get; set; } = 1;
        public static int PageSize { get; set; } = 10;
        public static Direction SortDirection { get; set; } = Direction.Desc;
        public static string? SortColumn { get; set; } = nameof(Entity.Id);
    }
}

using BizCardSystem.Domain.Abstractions;
using BizCardSystem.Domain.Shared.Filter.Configuration;
using BizCardSystem.Domain.Shared.Filter.Enums;

namespace BizCardSystem.Domain.Shared.Filter;

public class BaseFilter : IFilter
{
    private int _pageIndex;
    private int _pageSize;
    private string? _sortColumn;
    private Direction _sortDirection;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value == 0 ? DefaultFilterValue.PageIndex : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value == 0 ? DefaultFilterValue.PageSize : value;
    }

    public string? SortColumn
    {
        get => _sortColumn;
        set => _sortColumn = string.IsNullOrEmpty(value) ? nameof(Entity.Id) : value;
    }

    public Direction SortDirection
    {
        get => _sortDirection;
        set => _sortDirection = value == Direction.Asc ? DefaultFilterValue.SortDirection : value;
    }

    protected BaseFilter() => SetDefaultValues();

    private void SetDefaultValues()
    {
        _pageIndex = DefaultFilterValue.PageIndex;
        _pageSize = DefaultFilterValue.PageSize;
        _sortColumn = nameof(Entity.Id);
        _sortDirection = DefaultFilterValue.SortDirection;
    }
}
public interface IFilter
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public string? SortColumn { get; set; }
    public Direction SortDirection { get; set; }
}
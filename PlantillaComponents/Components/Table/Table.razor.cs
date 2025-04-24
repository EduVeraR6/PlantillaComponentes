using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.Table
{
    public partial class Table<TItem> : ComponentBase
    {
        [Parameter] public List<TItem> Data { get; set; } = new();
        [Parameter] public List<ColumnDef<TItem>> Columns { get; set; } = new();
        [Parameter] public bool Searchable { get; set; } = true;
        [Parameter] public string SearchPlaceholder { get; set; } = "Buscar...";
        [Parameter] public bool Loading { get; set; } = false;
        [Parameter] public int DefaultPage { get; set; } = 0;
        [Parameter] public int DefaultSize { get; set; } = 5;
        [Parameter] public bool ShowOptions { get; set; } = true;

        [Parameter]
        public bool ShowActions { get; set; } = false;

        [Parameter] public string TableClassName { get; set; }

        [Parameter] public Func<TItem, RenderFragment>? RowExpand { get; set; }
        [Parameter] public Func<TItem, bool>? DisableRowExpand { get; set; }
        [Parameter] public EventCallback<TItem> OnSelectAction { get; set; }
        [Parameter] public EventCallback<TItem> OnDeleteAction { get; set; }

        private int PageIndex = 0;
        private int PageSize = 5;
        private string GlobalFilter = string.Empty;
        private List<TItem> FilteredData => string.IsNullOrWhiteSpace(GlobalFilter)
            ? Data
            : Data.Where(item => item?.ToString()?.Contains(GlobalFilter, StringComparison.OrdinalIgnoreCase) ?? false).ToList();

        private int PageCount => (int)Math.Ceiling(FilteredData.Count / (double)PageSize);
        private List<TItem> PagedData => FilteredData.Skip(PageIndex * PageSize).Take(PageSize).ToList();
        private bool IsFirstPage => PageIndex == 0;
        private bool IsLastPage => PageIndex >= PageCount - 1;
        private List<int> PageSizeOptions = new() { 5, 10, 20 };

        private HashSet<TItem> ExpandedRows = new();
        private TItem? RowToDelete;
        private bool ShowDeleteConfirm = false;

        private void ToggleRow(TItem row)
        {
            if (DisableRowExpand?.Invoke(row) == true) return;
            if (!ExpandedRows.Add(row))
                ExpandedRows.Remove(row);
        }

        private void AskDelete(TItem row)
        {
            RowToDelete = row;
            ShowDeleteConfirm = true;
        }

        private async Task ConfirmDelete()
        {
            if (RowToDelete is not null && OnDeleteAction.HasDelegate)
                await OnDeleteAction.InvokeAsync(RowToDelete);

            RowToDelete = default;
            ShowDeleteConfirm = false;
        }

        private void CancelDelete() => ShowDeleteConfirm = false;
        private void FirstPage() => PageIndex = 0;
        private void LastPage() => PageIndex = PageCount - 1;
        private void NextPage() => PageIndex = Math.Min(PageCount - 1, PageIndex + 1);
        private void PreviousPage() => PageIndex = Math.Max(0, PageIndex - 1);
        private void OnPageSizeChanged(ChangeEventArgs e) => PageSize = int.Parse(e.Value?.ToString() ?? "5");

        private int TotalColSpan => Columns.Count + (RowExpand != null ? 1 : 0) + (ShowActions ? 1 : 0);
    }
}

public class ColumnDef<T>
{
    public string Header { get; set; }
    public Func<T, RenderFragment> Cell { get; set; }
    public string Meta { get; set; } = string.Empty;
}


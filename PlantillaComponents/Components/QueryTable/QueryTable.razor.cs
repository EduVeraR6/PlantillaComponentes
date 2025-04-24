using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PlantillaComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlantillaComponents.Components.QueryTable
{
    public partial class QueryTable<TItem> : ComponentBase
    {
        [Parameter] public List<ColumnDefinition> Columns { get; set; } = new();
        [Parameter] public string FetchUrl { get; set; }
        [Parameter] public string SearchUrl { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public Dictionary<string, object> QueryParams { get; set; } = new();
        [Parameter] public string FilterKey { get; set; } = "filter";
        [Parameter] public string PageKey { get; set; } = "page";
        [Parameter] public string SizeKey { get; set; } = "size";
        [Parameter] public string SortKey { get; set; } = "sort";
        [Parameter] public string ResponseDataKey { get; set; } = "content";
        [Parameter] public string ResponseTotalCount { get; set; } = "totalElements";
        [Parameter] public int DebounceDelay { get; set; } = 300;
        [Parameter] public bool ShowOptions { get; set; } = true;
        [Parameter] public bool Searchable { get; set; } = true;
        [Parameter] public EventCallback<TItem> OnSelectAction { get; set; }
        [Parameter] public EventCallback<TItem> OnDeleteAction { get; set; }
        [Parameter] public string StatusAccessor { get; set; }
        [Parameter] public EventCallback<(TItem, string)> OnStatusChange { get; set; }
        [Parameter] public EventCallback OnNewAction { get; set; }
        [Parameter] public EventCallback<List<TItem>> OnDeleteMassiveAction { get; set; }
        [Parameter] public int DefaultPage { get; set; } = 0;
        [Parameter] public int DefaultSize { get; set; } = 5;
        [Parameter] public string DefaultSortQuery { get; set; } = "";
        [Parameter] public bool Sortable { get; set; } = true;
        [Parameter] public int PagesToShow { get; set; } = 5;
        [Parameter] public string TableClassName { get; set; }
        [Parameter] public RenderFragment<TItem> RowExpand { get; set; }
        [Parameter] public Func<TItem, bool> DisableRowExpand { get; set; }
        [Parameter] public string NotFoundLabel { get; set; } = "No se encontraron resultados";
        [Parameter] public string RefreshEvent { get; set; }
        [Parameter] public string SearchPlaceholder { get; set; } = "Buscar...";

        private List<TItem> Data { get; set; } = new();
        private int TotalCount { get; set; } = 0;
        private bool Loading { get; set; } = true;
        private bool Error { get; set; } = false;
        private string GlobalFilter { get; set; } = "";
        private int PageIndex { get; set; }
        private int PageSize { get; set; }
        private string SortQuery { get; set; } = "";
        private Dictionary<string, bool> ExpandedRows { get; set; } = new();
        private List<TItem> SelectedRows { get; set; } = new();
        private bool ShowMassDeleteConfirmation { get; set; } = false;
        private bool IsMobile { get; set; } = false;
        private TItem OverlayData { get; set; }
        private (double Top, double Left) OverlayPosition { get; set; }
        private System.Threading.Timer _debounceTimer;
        private DotNetObjectReference<QueryTable<TItem>> _dotNetObjectRef;

        public class ColumnDefinition
        {
            public string Header { get; set; }
            public string Accessor { get; set; }
            public string ClassName { get; set; }
            public bool? EnableSorting { get; set; }
            public Func<TItem, object> Cell { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            _dotNetObjectRef = DotNetObjectReference.Create(this);
            PageIndex = DefaultPage;
            PageSize = DefaultSize;
            SortQuery = DefaultSortQuery;

            await CheckIfMobile();
            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(RefreshEvent))
                {
                    await JSRuntime.InvokeVoidAsync("registerTableRefreshEvent", _dotNetObjectRef, RefreshEvent);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("registerTableRefreshEvent", _dotNetObjectRef, "refreshTable");
                }
            }
        }

        [JSInvokable]
        public async Task RefreshTable()
        {
            await LoadData(true);
            StateHasChanged();
        }

        private async Task LoadData(bool forceRefresh = false)
        {
            try
            {
                Loading = true;
                StateHasChanged();

                var url = FetchUrl;
                var queryString = new List<string>();

                // Build query params
                foreach (var param in QueryParams)
                {
                    queryString.Add($"{param.Key}={param.Value}");
                }

                queryString.Add($"{PageKey}={PageIndex}");
                queryString.Add($"{SizeKey}={PageSize}");

                if (!string.IsNullOrEmpty(GlobalFilter))
                {
                    queryString.Add($"{FilterKey}={Uri.EscapeDataString(GlobalFilter)}");
                }

                if (!string.IsNullOrEmpty(SortQuery))
                {
                    queryString.Add($"{SortKey}={Uri.EscapeDataString(SortQuery)}");
                }

                if (queryString.Count > 0)
                {
                    url += (url.Contains("?") ? "&" : "?") + string.Join("&", queryString);
                }

                var response = await Http.GetFromJsonAsync<JsonElement>(url);

                try
                {
                    // Extract data and total count from response
                    if (response.TryGetProperty(ResponseDataKey, out var contentElement) &&
                        response.TryGetProperty(ResponseTotalCount, out var totalElement))
                    {
                        Data = contentElement.Deserialize<List<TItem>>();
                        TotalCount = totalElement.GetInt32();
                    }
                    else
                    {
                        Logger.LogError("Failed to extract data or total count from response");
                        Error = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error parsing response data");
                    Error = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading data");
                Error = true;
            }
            finally
            {
                Loading = false;
                StateHasChanged();
            }
        }

        private async Task CheckIfMobile()
        {
            IsMobile = await JSRuntime.InvokeAsync<bool>("window.innerWidth < 768");
        }

        private void HandleGlobalFilterChange(ChangeEventArgs e)
        {
            GlobalFilter = e.Value?.ToString() ?? "";
            PageIndex = 0;

            // Debounce the search
            _debounceTimer?.Dispose();
            _debounceTimer = new System.Threading.Timer(async _ =>
            {
                await InvokeAsync(async () =>
                {
                    await LoadData();
                    StateHasChanged();
                });
            }, null, DebounceDelay, Timeout.Infinite);
        }

        private void ClearGlobalFilter()
        {
            GlobalFilter = "";
            PageIndex = 0;
            _ = LoadData();
        }

        private void ToggleRowExpansion(string rowId, TItem item)
        {
            if (DisableRowExpand != null && DisableRowExpand(item))
                return;

            if (ExpandedRows.ContainsKey(rowId))
            {
                ExpandedRows[rowId] = !ExpandedRows[rowId];
            }
            else
            {
                ExpandedRows[rowId] = true;
            }
        }

        private void HandleRowSelection(TItem item)
        {
            if (SelectedRows.Contains(item))
            {
                SelectedRows.Remove(item);
            }
            else
            {
                SelectedRows.Add(item);
            }
        }

        private async Task HandleSort(string columnId, string direction)
        {
            ExpandedRows.Clear();
            PageIndex = 0;
            SortQuery = $"{columnId.Replace("_", ".")},{direction}";
            await LoadData();
        }

        private async Task HandleDeleteClick(TItem item, MouseEventArgs e)
        {
            var boundingRect = await JSRuntime.InvokeAsync<DOMRect>("getBoundingClientRect", e);
            OverlayData = item;
            OverlayPosition = (boundingRect.Bottom + boundingRect.Height + 10, boundingRect.Left + boundingRect.Width / 2);
        }

        private async Task ConfirmDelete()
        {
            if (OverlayData != null)
            {
                await OnDeleteAction.InvokeAsync(OverlayData);
                await LoadData(true);
            }
            OverlayData = default;
        }

        private void CancelDelete()
        {
            OverlayData = default;
        }

        private async Task HandleStatusToggle(TItem item)
        {
            if (string.IsNullOrEmpty(StatusAccessor) || !OnStatusChange.HasDelegate)
                return;

            var currentStatus = GetPropertyValue(item, StatusAccessor)?.ToString();
            var newStatus = currentStatus == "A" ? "I" : "A";

            // Call the status change callback
            await OnStatusChange.InvokeAsync((item, newStatus));

            // Update the local data
            await LoadData(true);
        }

        private async Task ConfirmMassDelete()
        {
            if (OnDeleteMassiveAction.HasDelegate && SelectedRows.Count > 0)
            {
                await OnDeleteMassiveAction.InvokeAsync(SelectedRows);
                await LoadData(true);
            }
            SelectedRows.Clear();
            ShowMassDeleteConfirmation = false;
        }

        private void NavigateToFirstPage()
        {
            if (PageIndex > 0)
            {
                PageIndex = 0;
                ExpandedRows.Clear();
                _ = LoadData();
            }
        }

        private void NavigateToPrevPage()
        {
            if (PageIndex > 0)
            {
                PageIndex--;
                ExpandedRows.Clear();
                _ = LoadData();
            }
        }

        private void NavigateToNextPage()
        {
            int totalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
            if (PageIndex < totalPages - 1)
            {
                PageIndex++;
                ExpandedRows.Clear();
                _ = LoadData();
            }
        }

        private void NavigateToPage(int page)
        {
            PageIndex = page;
            ExpandedRows.Clear();
            _ = LoadData();
        }

        private async Task HandlePageSizeChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize))
            {
                PageSize = newSize;
                PageIndex = 0;
                ExpandedRows.Clear();
                await LoadData();
            }
        }

        private int GetColumnCount()
        {
            int count = Columns.Count;
            if (RowExpand != null) count++;
            if (OnSelectAction.HasDelegate || OnDeleteAction.HasDelegate) count++;
            if (OnDeleteMassiveAction.HasDelegate) count++;
            return count;
        }

        private object GetPropertyValue(TItem item, string propertyName)
        {
            if (item == null || string.IsNullOrEmpty(propertyName))
                return null;

            return item.GetType().GetProperty(propertyName)?.GetValue(item);
        }

        private object GetCellValue(TItem item, ColumnDefinition column)
        {
            if (column.Cell != null)
            {
                return column.Cell(item);
            }

            return GetPropertyValue(item, column.Accessor);
        }

    }
}

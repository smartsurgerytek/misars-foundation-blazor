using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Web;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Misars.Foundation.App.Filemanagements;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace Misars.Foundation.App.Blazor.Client.Pages
{
    public partial class Filemanagements
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<FilemanagementDto> FilemanagementList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateFilemanagement { get; set; }
        private bool CanEditFilemanagement { get; set; }
        private bool CanDeleteFilemanagement { get; set; }
        private FilemanagementCreateDto NewFilemanagement { get; set; }
        private Validations NewFilemanagementValidations { get; set; } = new();
        private FilemanagementUpdateDto EditingFilemanagement { get; set; }
        private Validations EditingFilemanagementValidations { get; set; } = new();
        private Guid EditingFilemanagementId { get; set; }
        private Modal CreateFilemanagementModal { get; set; } = new();
        private Modal EditFilemanagementModal { get; set; } = new();
        private GetFilemanagementsInput Filter { get; set; }
        private DataGridEntityActionsColumn<FilemanagementDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "filemanagement-create-tab";
        protected string SelectedEditTab = "filemanagement-edit-tab";
        private FilemanagementDto? SelectedFilemanagement;
        
        
        
        
        
        private List<FilemanagementDto> SelectedFilemanagements { get; set; } = new();
        private bool AllFilemanagementsSelected { get; set; }
        
        public Filemanagements()
        {
            NewFilemanagement = new FilemanagementCreateDto();
            EditingFilemanagement = new FilemanagementUpdateDto();
            Filter = new GetFilemanagementsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            FilemanagementList = new List<FilemanagementDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                await SetBreadcrumbItemsAsync();
                await SetToolbarItemsAsync();
                await InvokeAsync(StateHasChanged);
            }
        }  

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Filemanagements"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewFilemanagement"], async () =>
            {
                await OpenCreateFilemanagementModalAsync();
            }, IconName.Add, requiredPolicyName: AppPermissions.Filemanagements.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateFilemanagement = await AuthorizationService
                .IsGrantedAsync(AppPermissions.Filemanagements.Create);
            CanEditFilemanagement = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.Filemanagements.Edit);
            CanDeleteFilemanagement = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.Filemanagements.Delete);
                            
                            
        }

        private async Task GetFilemanagementsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await FilemanagementsAppService.GetListAsync(Filter);
            FilemanagementList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetFilemanagementsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await FilemanagementsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("App") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/filemanagements/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&FileName={HttpUtility.UrlEncode(Filter.FileName)}&FilePath={HttpUtility.UrlEncode(Filter.FilePath)}&UploadDateMin={Filter.UploadDateMin?.ToString("O")}&UploadDateMax={Filter.UploadDateMax?.ToString("O")}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<FilemanagementDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetFilemanagementsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateFilemanagementModalAsync()
        {
            NewFilemanagement = new FilemanagementCreateDto{
                UploadDate = DateTime.Now,

                
            };

            SelectedCreateTab = "filemanagement-create-tab";
            
            
            await NewFilemanagementValidations.ClearAll();
            await CreateFilemanagementModal.Show();
        }

        private async Task CloseCreateFilemanagementModalAsync()
        {
            NewFilemanagement = new FilemanagementCreateDto{
                UploadDate = DateTime.Now,

                
            };
            await CreateFilemanagementModal.Hide();
        }

        private async Task OpenEditFilemanagementModalAsync(FilemanagementDto input)
        {
            SelectedEditTab = "filemanagement-edit-tab";
            
            
            var filemanagement = await FilemanagementsAppService.GetAsync(input.Id);
            
            EditingFilemanagementId = filemanagement.Id;
            EditingFilemanagement = ObjectMapper.Map<FilemanagementDto, FilemanagementUpdateDto>(filemanagement);
            
            await EditingFilemanagementValidations.ClearAll();
            await EditFilemanagementModal.Show();
        }

        private async Task DeleteFilemanagementAsync(FilemanagementDto input)
        {
            await FilemanagementsAppService.DeleteAsync(input.Id);
            await GetFilemanagementsAsync();
        }

        private async Task CreateFilemanagementAsync()
        {
            try
            {
                if (await NewFilemanagementValidations.ValidateAll() == false)
                {
                    return;
                }

                await FilemanagementsAppService.CreateAsync(NewFilemanagement);
                await GetFilemanagementsAsync();
                await CloseCreateFilemanagementModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditFilemanagementModalAsync()
        {
            await EditFilemanagementModal.Hide();
        }

        private async Task UpdateFilemanagementAsync()
        {
            try
            {
                if (await EditingFilemanagementValidations.ValidateAll() == false)
                {
                    return;
                }

                await FilemanagementsAppService.UpdateAsync(EditingFilemanagementId, EditingFilemanagement);
                await GetFilemanagementsAsync();
                await EditFilemanagementModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }









        protected virtual async Task OnFileNameChangedAsync(string? fileName)
        {
            Filter.FileName = fileName;
            await SearchAsync();
        }
        protected virtual async Task OnFilePathChangedAsync(string? filePath)
        {
            Filter.FilePath = filePath;
            await SearchAsync();
        }
        protected virtual async Task OnUploadDateMinChangedAsync(DateTime? uploadDateMin)
        {
            Filter.UploadDateMin = uploadDateMin.HasValue ? uploadDateMin.Value.Date : uploadDateMin;
            await SearchAsync();
        }
        protected virtual async Task OnUploadDateMaxChangedAsync(DateTime? uploadDateMax)
        {
            Filter.UploadDateMax = uploadDateMax.HasValue ? uploadDateMax.Value.Date.AddDays(1).AddSeconds(-1) : uploadDateMax;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllFilemanagementsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllFilemanagementsSelected = false;
            SelectedFilemanagements.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedFilemanagementRowsChanged()
        {
            if (SelectedFilemanagements.Count != PageSize)
            {
                AllFilemanagementsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedFilemanagementsAsync()
        {
            var message = AllFilemanagementsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedFilemanagements.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllFilemanagementsSelected)
            {
                await FilemanagementsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await FilemanagementsAppService.DeleteByIdsAsync(SelectedFilemanagements.Select(x => x.Id).ToList());
            }

            SelectedFilemanagements.Clear();
            AllFilemanagementsSelected = false;

            await GetFilemanagementsAsync();
        }


    }
}

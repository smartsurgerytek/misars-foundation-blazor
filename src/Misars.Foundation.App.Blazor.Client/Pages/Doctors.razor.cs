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
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace Misars.Foundation.App.Blazor.Client.Pages
{
    public partial class Doctors
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<DoctorDto> DoctorList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateDoctor { get; set; }
        private bool CanEditDoctor { get; set; }
        private bool CanDeleteDoctor { get; set; }
        private DoctorCreateDto NewDoctor { get; set; }
        private Validations NewDoctorValidations { get; set; } = new();
        private DoctorUpdateDto EditingDoctor { get; set; }
        private Validations EditingDoctorValidations { get; set; } = new();
        private Guid EditingDoctorId { get; set; }
        private Modal CreateDoctorModal { get; set; } = new();
        private Modal EditDoctorModal { get; set; } = new();
        private GetDoctorsInput Filter { get; set; }
        private DataGridEntityActionsColumn<DoctorDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "doctor-create-tab";
        protected string SelectedEditTab = "doctor-edit-tab";
        private DoctorDto? SelectedDoctor;
        
        
        
        
        
        private List<DoctorDto> SelectedDoctors { get; set; } = new();
        private bool AllDoctorsSelected { get; set; }
        
        public Doctors()
        {
            NewDoctor = new DoctorCreateDto();
            EditingDoctor = new DoctorUpdateDto();
            Filter = new GetDoctorsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            DoctorList = new List<DoctorDto>();
            
            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Doctors"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewDoctor"], async () =>
            {
                await OpenCreateDoctorModalAsync();
            }, IconName.Add, requiredPolicyName: AppPermissions.Doctors.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateDoctor = await AuthorizationService
                .IsGrantedAsync(AppPermissions.Doctors.Create);
            CanEditDoctor = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.Doctors.Edit);
            CanDeleteDoctor = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.Doctors.Delete);
                            
                            
        }

        private async Task GetDoctorsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await DoctorsAppService.GetListAsync(Filter);
            DoctorList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetDoctorsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await DoctorsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("App") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/doctors/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&name={HttpUtility.UrlEncode(Filter.name)}&DoctorID={HttpUtility.UrlEncode(Filter.DoctorID)}&Specialty={HttpUtility.UrlEncode(Filter.Specialty)}&Department={HttpUtility.UrlEncode(Filter.Department)}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<DoctorDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetDoctorsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateDoctorModalAsync()
        {
            NewDoctor = new DoctorCreateDto{
                
                
            };

            SelectedCreateTab = "doctor-create-tab";
            
            
            await NewDoctorValidations.ClearAll();
            await CreateDoctorModal.Show();
        }

        private async Task CloseCreateDoctorModalAsync()
        {
            NewDoctor = new DoctorCreateDto{
                
                
            };
            await CreateDoctorModal.Hide();
        }

        private async Task OpenEditDoctorModalAsync(DoctorDto input)
        {
            SelectedEditTab = "doctor-edit-tab";
            
            
            var doctor = await DoctorsAppService.GetAsync(input.Id);
            
            EditingDoctorId = doctor.Id;
            EditingDoctor = ObjectMapper.Map<DoctorDto, DoctorUpdateDto>(doctor);
            
            await EditingDoctorValidations.ClearAll();
            await EditDoctorModal.Show();
        }

        private async Task DeleteDoctorAsync(DoctorDto input)
        {
            await DoctorsAppService.DeleteAsync(input.Id);
            await GetDoctorsAsync();
        }

        private async Task CreateDoctorAsync()
        {
            try
            {
                if (await NewDoctorValidations.ValidateAll() == false)
                {
                    return;
                }

                await DoctorsAppService.CreateAsync(NewDoctor);
                await GetDoctorsAsync();
                await CloseCreateDoctorModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditDoctorModalAsync()
        {
            await EditDoctorModal.Hide();
        }

        private async Task UpdateDoctorAsync()
        {
            try
            {
                if (await EditingDoctorValidations.ValidateAll() == false)
                {
                    return;
                }

                await DoctorsAppService.UpdateAsync(EditingDoctorId, EditingDoctor);
                await GetDoctorsAsync();
                await EditDoctorModal.Hide();                
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









        protected virtual async Task OnnameChangedAsync(string? name)
        {
            Filter.name = name;
            await SearchAsync();
        }
        protected virtual async Task OnDoctorIDChangedAsync(string? doctorID)
        {
            Filter.DoctorID = doctorID;
            await SearchAsync();
        }
        protected virtual async Task OnSpecialtyChangedAsync(string? specialty)
        {
            Filter.Specialty = specialty;
            await SearchAsync();
        }
        protected virtual async Task OnDepartmentChangedAsync(string? department)
        {
            Filter.Department = department;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllDoctorsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllDoctorsSelected = false;
            SelectedDoctors.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedDoctorRowsChanged()
        {
            if (SelectedDoctors.Count != PageSize)
            {
                AllDoctorsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedDoctorsAsync()
        {
            var message = AllDoctorsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedDoctors.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllDoctorsSelected)
            {
                await DoctorsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await DoctorsAppService.DeleteByIdsAsync(SelectedDoctors.Select(x => x.Id).ToList());
            }

            SelectedDoctors.Clear();
            AllDoctorsSelected = false;

            await GetDoctorsAsync();
        }


    }
}

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
using Misars.Foundation.App.SurgeryTimetables;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace Misars.Foundation.App.Blazor.Client.Pages
{
    public partial class SurgeryTimetables
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<SurgeryTimetableWithNavigationPropertiesDto> SurgeryTimetableList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateSurgeryTimetable { get; set; }
        private bool CanEditSurgeryTimetable { get; set; }
        private bool CanDeleteSurgeryTimetable { get; set; }
        private SurgeryTimetableCreateDto NewSurgeryTimetable { get; set; }
        private Validations NewSurgeryTimetableValidations { get; set; } = new();
        private SurgeryTimetableUpdateDto EditingSurgeryTimetable { get; set; }
        private Validations EditingSurgeryTimetableValidations { get; set; } = new();
        private Guid EditingSurgeryTimetableId { get; set; }
        private Modal CreateSurgeryTimetableModal { get; set; } = new();
        private Modal EditSurgeryTimetableModal { get; set; } = new();
        private GetSurgeryTimetablesInput Filter { get; set; }
        private DataGridEntityActionsColumn<SurgeryTimetableWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "surgeryTimetable-create-tab";
        protected string SelectedEditTab = "surgeryTimetable-edit-tab";
        private SurgeryTimetableWithNavigationPropertiesDto? SelectedSurgeryTimetable;
        private IReadOnlyList<LookupDto<Guid>> DoctorsCollection { get; set; } = new List<LookupDto<Guid>>();
private IReadOnlyList<LookupDto<Guid>> PatientsCollection { get; set; } = new List<LookupDto<Guid>>();

        
        
        
        
        private List<SurgeryTimetableWithNavigationPropertiesDto> SelectedSurgeryTimetables { get; set; } = new();
        private bool AllSurgeryTimetablesSelected { get; set; }
        
        public SurgeryTimetables()
        {
            NewSurgeryTimetable = new SurgeryTimetableCreateDto();
            EditingSurgeryTimetable = new SurgeryTimetableUpdateDto();
            Filter = new GetSurgeryTimetablesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            SurgeryTimetableList = new List<SurgeryTimetableWithNavigationPropertiesDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetDoctorCollectionLookupAsync();


            await GetPatientCollectionLookupAsync();


            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["SurgeryTimetables"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewSurgeryTimetable"], async () =>
            {
                await OpenCreateSurgeryTimetableModalAsync();
            }, IconName.Add, requiredPolicyName: AppPermissions.SurgeryTimetables.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateSurgeryTimetable = await AuthorizationService
                .IsGrantedAsync(AppPermissions.SurgeryTimetables.Create);
            CanEditSurgeryTimetable = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.SurgeryTimetables.Edit);
            CanDeleteSurgeryTimetable = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.SurgeryTimetables.Delete);
                            
                            
        }

        private async Task GetSurgeryTimetablesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await SurgeryTimetablesAppService.GetListAsync(Filter);
            SurgeryTimetableList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetSurgeryTimetablesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await SurgeryTimetablesAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("App") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/surgery-timetables/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&startdateMin={Filter.startdateMin?.ToString("O")}&startdateMax={Filter.startdateMax?.ToString("O")}&enddateMin={Filter.enddateMin?.ToString("O")}&enddateMax={Filter.enddateMax?.ToString("O")}&Time={HttpUtility.UrlEncode(Filter.Time)}&Department={HttpUtility.UrlEncode(Filter.Department)}&Diagnosis={HttpUtility.UrlEncode(Filter.Diagnosis)}&SurgicalMethod={HttpUtility.UrlEncode(Filter.SurgicalMethod)}&notes={HttpUtility.UrlEncode(Filter.notes)}&DoctorId={Filter.DoctorId}&PatientId={Filter.PatientId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<SurgeryTimetableWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetSurgeryTimetablesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateSurgeryTimetableModalAsync()
        {
            NewSurgeryTimetable = new SurgeryTimetableCreateDto{
                startdate = DateTime.Now,
enddate = DateTime.Now,

                
            };

            SelectedCreateTab = "surgeryTimetable-create-tab";
            
            
            await NewSurgeryTimetableValidations.ClearAll();
            await CreateSurgeryTimetableModal.Show();
        }

        private async Task CloseCreateSurgeryTimetableModalAsync()
        {
            NewSurgeryTimetable = new SurgeryTimetableCreateDto{
                startdate = DateTime.Now,
enddate = DateTime.Now,

                
            };
            await CreateSurgeryTimetableModal.Hide();
        }

        private async Task OpenEditSurgeryTimetableModalAsync(SurgeryTimetableWithNavigationPropertiesDto input)
        {
            SelectedEditTab = "surgeryTimetable-edit-tab";
            
            
            var surgeryTimetable = await SurgeryTimetablesAppService.GetWithNavigationPropertiesAsync(input.SurgeryTimetable.Id);
            
            EditingSurgeryTimetableId = surgeryTimetable.SurgeryTimetable.Id;
            EditingSurgeryTimetable = ObjectMapper.Map<SurgeryTimetableDto, SurgeryTimetableUpdateDto>(surgeryTimetable.SurgeryTimetable);
            
            await EditingSurgeryTimetableValidations.ClearAll();
            await EditSurgeryTimetableModal.Show();
        }

        private async Task DeleteSurgeryTimetableAsync(SurgeryTimetableWithNavigationPropertiesDto input)
        {
            await SurgeryTimetablesAppService.DeleteAsync(input.SurgeryTimetable.Id);
            await GetSurgeryTimetablesAsync();
        }

        private async Task CreateSurgeryTimetableAsync()
        {
            try
            {
                if (await NewSurgeryTimetableValidations.ValidateAll() == false)
                {
                    return;
                }

                await SurgeryTimetablesAppService.CreateAsync(NewSurgeryTimetable);
                await GetSurgeryTimetablesAsync();
                await CloseCreateSurgeryTimetableModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditSurgeryTimetableModalAsync()
        {
            await EditSurgeryTimetableModal.Hide();
        }

        private async Task UpdateSurgeryTimetableAsync()
        {
            try
            {
                if (await EditingSurgeryTimetableValidations.ValidateAll() == false)
                {
                    return;
                }

                await SurgeryTimetablesAppService.UpdateAsync(EditingSurgeryTimetableId, EditingSurgeryTimetable);
                await GetSurgeryTimetablesAsync();
                await EditSurgeryTimetableModal.Hide();                
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









        protected virtual async Task OnstartdateMinChangedAsync(DateTime? startdateMin)
        {
            Filter.startdateMin = startdateMin.HasValue ? startdateMin.Value.Date : startdateMin;
            await SearchAsync();
        }
        protected virtual async Task OnstartdateMaxChangedAsync(DateTime? startdateMax)
        {
            Filter.startdateMax = startdateMax.HasValue ? startdateMax.Value.Date.AddDays(1).AddSeconds(-1) : startdateMax;
            await SearchAsync();
        }
        protected virtual async Task OnenddateMinChangedAsync(DateTime? enddateMin)
        {
            Filter.enddateMin = enddateMin.HasValue ? enddateMin.Value.Date : enddateMin;
            await SearchAsync();
        }
        protected virtual async Task OnenddateMaxChangedAsync(DateTime? enddateMax)
        {
            Filter.enddateMax = enddateMax.HasValue ? enddateMax.Value.Date.AddDays(1).AddSeconds(-1) : enddateMax;
            await SearchAsync();
        }
        protected virtual async Task OnTimeChangedAsync(string? time)
        {
            Filter.Time = time;
            await SearchAsync();
        }
        protected virtual async Task OnDepartmentChangedAsync(string? department)
        {
            Filter.Department = department;
            await SearchAsync();
        }
        protected virtual async Task OnDiagnosisChangedAsync(string? diagnosis)
        {
            Filter.Diagnosis = diagnosis;
            await SearchAsync();
        }
        protected virtual async Task OnSurgicalMethodChangedAsync(string? surgicalMethod)
        {
            Filter.SurgicalMethod = surgicalMethod;
            await SearchAsync();
        }
        protected virtual async Task OnnotesChangedAsync(string? notes)
        {
            Filter.notes = notes;
            await SearchAsync();
        }
        protected virtual async Task OnDoctorIdChangedAsync(Guid? doctorId)
        {
            Filter.DoctorId = doctorId;
            await SearchAsync();
        }
        protected virtual async Task OnPatientIdChangedAsync(Guid? patientId)
        {
            Filter.PatientId = patientId;
            await SearchAsync();
        }
        

        private async Task GetDoctorCollectionLookupAsync(string? newValue = null)
        {
            DoctorsCollection = (await SurgeryTimetablesAppService.GetDoctorLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private async Task GetPatientCollectionLookupAsync(string? newValue = null)
        {
            PatientsCollection = (await SurgeryTimetablesAppService.GetPatientLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }





        private Task SelectAllItems()
        {
            AllSurgeryTimetablesSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllSurgeryTimetablesSelected = false;
            SelectedSurgeryTimetables.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedSurgeryTimetableRowsChanged()
        {
            if (SelectedSurgeryTimetables.Count != PageSize)
            {
                AllSurgeryTimetablesSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedSurgeryTimetablesAsync()
        {
            var message = AllSurgeryTimetablesSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedSurgeryTimetables.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllSurgeryTimetablesSelected)
            {
                await SurgeryTimetablesAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await SurgeryTimetablesAppService.DeleteByIdsAsync(SelectedSurgeryTimetables.Select(x => x.SurgeryTimetable.Id).ToList());
            }

            SelectedSurgeryTimetables.Clear();
            AllSurgeryTimetablesSelected = false;

            await GetSurgeryTimetablesAsync();
        }


    }
}

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
using Misars.Foundation.App.Patients;
using Misars.Foundation.App.Permissions;
using Misars.Foundation.App.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Volo.Abp;
using Volo.Abp.Content;



namespace Misars.Foundation.App.Blazor.Client.Pages
{
    public partial class Patients
    {
        
        
            
        
            
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<PatientDto> PatientList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreatePatient { get; set; }
        private bool CanEditPatient { get; set; }
        private bool CanDeletePatient { get; set; }
        private PatientCreateDto NewPatient { get; set; }
        private Validations NewPatientValidations { get; set; } = new();
        private PatientUpdateDto EditingPatient { get; set; }
        private Validations EditingPatientValidations { get; set; } = new();
        private Guid EditingPatientId { get; set; }
        private Modal CreatePatientModal { get; set; } = new();
        private Modal EditPatientModal { get; set; } = new();
        private GetPatientsInput Filter { get; set; }
        private DataGridEntityActionsColumn<PatientDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "patient-create-tab";
        protected string SelectedEditTab = "patient-edit-tab";
        private PatientDto? SelectedPatient;
        
        
        
        
        
        private List<PatientDto> SelectedPatients { get; set; } = new();
        private bool AllPatientsSelected { get; set; }
        
        public Patients()
        {
            NewPatient = new PatientCreateDto();
            EditingPatient = new PatientUpdateDto();
            Filter = new GetPatientsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            PatientList = new List<PatientDto>();
            
            
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Patients"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewPatient"], async () =>
            {
                await OpenCreatePatientModalAsync();
            }, IconName.Add, requiredPolicyName: AppPermissions.Patients.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreatePatient = await AuthorizationService
                .IsGrantedAsync(AppPermissions.Patients.Create);
            CanEditPatient = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.Patients.Edit);
            CanDeletePatient = await AuthorizationService
                            .IsGrantedAsync(AppPermissions.Patients.Delete);
                            
                            
        }

        private async Task GetPatientsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await PatientsAppService.GetListAsync(Filter);
            PatientList = result.Items;
            TotalCount = (int)result.TotalCount;
            
            await ClearSelection();
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetPatientsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await PatientsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("App") ?? await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/patients/as-excel-file?DownloadToken={token}&FilterText={HttpUtility.UrlEncode(Filter.FilterText)}{culture}&name={HttpUtility.UrlEncode(Filter.name)}&PatientID={HttpUtility.UrlEncode(Filter.PatientID)}&DateofBirthMin={Filter.DateofBirthMin?.ToString("O")}&DateofBirthMax={Filter.DateofBirthMax?.ToString("O")}&Gender={HttpUtility.UrlEncode(Filter.Gender)}&MedicalHistory={HttpUtility.UrlEncode(Filter.MedicalHistory)}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PatientDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetPatientsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreatePatientModalAsync()
        {
            NewPatient = new PatientCreateDto{
                DateofBirth = DateTime.Now,

                
            };

            SelectedCreateTab = "patient-create-tab";
            
            
            await NewPatientValidations.ClearAll();
            await CreatePatientModal.Show();
        }

        private async Task CloseCreatePatientModalAsync()
        {
            NewPatient = new PatientCreateDto{
                DateofBirth = DateTime.Now,

                
            };
            await CreatePatientModal.Hide();
        }

        private async Task OpenEditPatientModalAsync(PatientDto input)
        {
            SelectedEditTab = "patient-edit-tab";
            
            
            var patient = await PatientsAppService.GetAsync(input.Id);
            
            EditingPatientId = patient.Id;
            EditingPatient = ObjectMapper.Map<PatientDto, PatientUpdateDto>(patient);
            
            await EditingPatientValidations.ClearAll();
            await EditPatientModal.Show();
        }

        private async Task DeletePatientAsync(PatientDto input)
        {
            await PatientsAppService.DeleteAsync(input.Id);
            await GetPatientsAsync();
        }

        private async Task CreatePatientAsync()
        {
            try
            {
                if (await NewPatientValidations.ValidateAll() == false)
                {
                    return;
                }

                await PatientsAppService.CreateAsync(NewPatient);
                await GetPatientsAsync();
                await CloseCreatePatientModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditPatientModalAsync()
        {
            await EditPatientModal.Hide();
        }

        private async Task UpdatePatientAsync()
        {
            try
            {
                if (await EditingPatientValidations.ValidateAll() == false)
                {
                    return;
                }

                await PatientsAppService.UpdateAsync(EditingPatientId, EditingPatient);
                await GetPatientsAsync();
                await EditPatientModal.Hide();                
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
        protected virtual async Task OnPatientIDChangedAsync(string? patientID)
        {
            Filter.PatientID = patientID;
            await SearchAsync();
        }
        protected virtual async Task OnDateofBirthMinChangedAsync(DateTime? dateofBirthMin)
        {
            Filter.DateofBirthMin = dateofBirthMin.HasValue ? dateofBirthMin.Value.Date : dateofBirthMin;
            await SearchAsync();
        }
        protected virtual async Task OnDateofBirthMaxChangedAsync(DateTime? dateofBirthMax)
        {
            Filter.DateofBirthMax = dateofBirthMax.HasValue ? dateofBirthMax.Value.Date.AddDays(1).AddSeconds(-1) : dateofBirthMax;
            await SearchAsync();
        }
        protected virtual async Task OnGenderChangedAsync(string? gender)
        {
            Filter.Gender = gender;
            await SearchAsync();
        }
        protected virtual async Task OnMedicalHistoryChangedAsync(string? medicalHistory)
        {
            Filter.MedicalHistory = medicalHistory;
            await SearchAsync();
        }
        





        private Task SelectAllItems()
        {
            AllPatientsSelected = true;
            
            return Task.CompletedTask;
        }

        private Task ClearSelection()
        {
            AllPatientsSelected = false;
            SelectedPatients.Clear();
            
            return Task.CompletedTask;
        }

        private Task SelectedPatientRowsChanged()
        {
            if (SelectedPatients.Count != PageSize)
            {
                AllPatientsSelected = false;
            }
            
            return Task.CompletedTask;
        }

        private async Task DeleteSelectedPatientsAsync()
        {
            var message = AllPatientsSelected ? L["DeleteAllRecords"].Value : L["DeleteSelectedRecords", SelectedPatients.Count].Value;
            
            if (!await UiMessageService.Confirm(message))
            {
                return;
            }

            if (AllPatientsSelected)
            {
                await PatientsAppService.DeleteAllAsync(Filter);
            }
            else
            {
                await PatientsAppService.DeleteByIdsAsync(SelectedPatients.Select(x => x.Id).ToList());
            }

            SelectedPatients.Clear();
            AllPatientsSelected = false;

            await GetPatientsAsync();
        }


    }
}

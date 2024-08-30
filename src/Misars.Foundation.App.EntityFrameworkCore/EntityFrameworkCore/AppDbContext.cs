using Misars.Foundation.App.Filemanagements;
using Misars.Foundation.App.SurgeryTimetables;
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.Patients;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace Misars.Foundation.App.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class AppDbContext :
    AbpDbContext<AppDbContext>,
    IIdentityProDbContext,
    ISaasDbContext
{
    public DbSet<Filemanagement> Filemanagements { get; set; } = null!;
    public DbSet<SurgeryTimetable> SurgeryTimetables { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureGdpr();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(AppConsts.DbTablePrefix + "YourEntities", AppConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        builder.ConfigureFileManagement();
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Filemanagement>(b =>
            {
                b.ToTable(AppConsts.DbTablePrefix + "Filemanagements", AppConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FileName).HasColumnName(nameof(Filemanagement.FileName)).HasMaxLength(FilemanagementConsts.FileNameMaxLength);
                b.Property(x => x.FilePath).HasColumnName(nameof(Filemanagement.FilePath)).HasMaxLength(FilemanagementConsts.FilePathMaxLength);
                b.Property(x => x.UploadDate).HasColumnName(nameof(Filemanagement.UploadDate));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Doctor>(b =>
            {
                b.ToTable(AppConsts.DbTablePrefix + "Doctors", AppConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.name).HasColumnName(nameof(Doctor.name)).HasMaxLength(DoctorConsts.nameMaxLength);
                b.Property(x => x.DoctorID).HasColumnName(nameof(Doctor.DoctorID)).HasMaxLength(DoctorConsts.DoctorIDMaxLength);
                b.Property(x => x.Specialty).HasColumnName(nameof(Doctor.Specialty)).HasMaxLength(DoctorConsts.SpecialtyMaxLength);
                b.Property(x => x.Department).HasColumnName(nameof(Doctor.Department)).HasMaxLength(DoctorConsts.DepartmentMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Patient>(b =>
            {
                b.ToTable(AppConsts.DbTablePrefix + "Patients", AppConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.name).HasColumnName(nameof(Patient.name)).HasMaxLength(PatientConsts.nameMaxLength);
                b.Property(x => x.PatientID).HasColumnName(nameof(Patient.PatientID)).HasMaxLength(PatientConsts.PatientIDMaxLength);
                b.Property(x => x.DateofBirth).HasColumnName(nameof(Patient.DateofBirth));
                b.Property(x => x.Gender).HasColumnName(nameof(Patient.Gender)).HasMaxLength(PatientConsts.GenderMaxLength);
                b.Property(x => x.MedicalHistory).HasColumnName(nameof(Patient.MedicalHistory)).HasMaxLength(PatientConsts.MedicalHistoryMaxLength);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<SurgeryTimetable>(b =>
            {
                b.ToTable(AppConsts.DbTablePrefix + "SurgeryTimetables", AppConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.startdate).HasColumnName(nameof(SurgeryTimetable.startdate));
                b.Property(x => x.enddate).HasColumnName(nameof(SurgeryTimetable.enddate));
                b.Property(x => x.Time).HasColumnName(nameof(SurgeryTimetable.Time)).HasMaxLength(SurgeryTimetableConsts.TimeMaxLength);
                b.Property(x => x.Department).HasColumnName(nameof(SurgeryTimetable.Department));
                b.Property(x => x.Diagnosis).HasColumnName(nameof(SurgeryTimetable.Diagnosis)).HasMaxLength(SurgeryTimetableConsts.DiagnosisMaxLength);
                b.Property(x => x.SurgicalMethod).HasColumnName(nameof(SurgeryTimetable.SurgicalMethod)).HasMaxLength(SurgeryTimetableConsts.SurgicalMethodMaxLength);
                b.Property(x => x.notes).HasColumnName(nameof(SurgeryTimetable.notes)).HasMaxLength(SurgeryTimetableConsts.notesMaxLength);
                b.HasOne<Doctor>().WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne<Patient>().WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.SetNull);
            });

        }
    }
}
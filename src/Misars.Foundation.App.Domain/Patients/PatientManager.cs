using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientManagerBase : DomainService
    {
        protected IPatientRepository _patientRepository;

        public PatientManagerBase(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public virtual async Task<Patient> CreateAsync(
        DateTime dateofBirth, string? name = null, string? patientID = null, string? gender = null, string? medicalHistory = null)
        {
            Check.NotNull(dateofBirth, nameof(dateofBirth));
            Check.Length(name, nameof(name), PatientConsts.nameMaxLength, PatientConsts.nameMinLength);
            Check.Length(patientID, nameof(patientID), PatientConsts.PatientIDMaxLength, PatientConsts.PatientIDMinLength);
            Check.Length(gender, nameof(gender), PatientConsts.GenderMaxLength, PatientConsts.GenderMinLength);
            Check.Length(medicalHistory, nameof(medicalHistory), PatientConsts.MedicalHistoryMaxLength, PatientConsts.MedicalHistoryMinLength);

            var patient = new Patient(
             GuidGenerator.Create(),
             dateofBirth, name, patientID, gender, medicalHistory
             );

            return await _patientRepository.InsertAsync(patient);
        }

        public virtual async Task<Patient> UpdateAsync(
            Guid id,
            DateTime dateofBirth, string? name = null, string? patientID = null, string? gender = null, string? medicalHistory = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(dateofBirth, nameof(dateofBirth));
            Check.Length(name, nameof(name), PatientConsts.nameMaxLength, PatientConsts.nameMinLength);
            Check.Length(patientID, nameof(patientID), PatientConsts.PatientIDMaxLength, PatientConsts.PatientIDMinLength);
            Check.Length(gender, nameof(gender), PatientConsts.GenderMaxLength, PatientConsts.GenderMinLength);
            Check.Length(medicalHistory, nameof(medicalHistory), PatientConsts.MedicalHistoryMaxLength, PatientConsts.MedicalHistoryMinLength);

            var patient = await _patientRepository.GetAsync(id);

            patient.DateofBirth = dateofBirth;
            patient.name = name;
            patient.PatientID = patientID;
            patient.Gender = gender;
            patient.MedicalHistory = medicalHistory;

            patient.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _patientRepository.UpdateAsync(patient);
        }

    }
}
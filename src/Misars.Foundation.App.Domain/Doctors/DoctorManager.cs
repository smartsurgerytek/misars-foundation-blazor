using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorManagerBase : DomainService
    {
        protected IDoctorRepository _doctorRepository;

        public DoctorManagerBase(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public virtual async Task<Doctor> CreateAsync(
        string? name = null, string? doctorID = null, string? specialty = null, string? department = null)
        {
            Check.Length(name, nameof(name), DoctorConsts.nameMaxLength, DoctorConsts.nameMinLength);
            Check.Length(doctorID, nameof(doctorID), DoctorConsts.DoctorIDMaxLength);
            Check.Length(specialty, nameof(specialty), DoctorConsts.SpecialtyMaxLength);
            Check.Length(department, nameof(department), DoctorConsts.DepartmentMaxLength, DoctorConsts.DepartmentMinLength);

            var doctor = new Doctor(
             GuidGenerator.Create(),
             name, doctorID, specialty, department
             );

            return await _doctorRepository.InsertAsync(doctor);
        }

        public virtual async Task<Doctor> UpdateAsync(
            Guid id,
            string? name = null, string? doctorID = null, string? specialty = null, string? department = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.Length(name, nameof(name), DoctorConsts.nameMaxLength, DoctorConsts.nameMinLength);
            Check.Length(doctorID, nameof(doctorID), DoctorConsts.DoctorIDMaxLength);
            Check.Length(specialty, nameof(specialty), DoctorConsts.SpecialtyMaxLength);
            Check.Length(department, nameof(department), DoctorConsts.DepartmentMaxLength, DoctorConsts.DepartmentMinLength);

            var doctor = await _doctorRepository.GetAsync(id);

            doctor.name = name;
            doctor.DoctorID = doctorID;
            doctor.Specialty = specialty;
            doctor.Department = department;

            doctor.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _doctorRepository.UpdateAsync(doctor);
        }

    }
}
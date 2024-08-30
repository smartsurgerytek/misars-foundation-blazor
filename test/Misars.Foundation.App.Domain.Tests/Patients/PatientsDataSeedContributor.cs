using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Misars.Foundation.App.Patients;

namespace Misars.Foundation.App.Patients
{
    public class PatientsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PatientsDataSeedContributor(IPatientRepository patientRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _patientRepository = patientRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _patientRepository.InsertAsync(new Patient
            (
                id: Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"),
                name: "06f8dc332c73495f971e62e4f9343801f9f2812825534119a13523d8623c47fac9e75048a4f84a1ca03a78a375856441838a",
                patientID: "75fa39fb48f141f58dc28764259defd4002da9a66ea1467bbb65c9d3f2f09bc9b938f75f24ca4763a0c3206f876801397fbf",
                dateofBirth: new DateTime(2001, 2, 7),
                gender: "ae1fe9a9a054437ca3ec8ee64922386f16b683708e2b45a5a1ec44d14899b2b90916b4f5b70d4908bb4c45f872c25d95e661",
                medicalHistory: "4affb9e6526749e499533a29769981dd60159865815642e8b64cf865bf3c70e31757f71ff66745feb9681a82a3ce240db5f4"
            ));

            await _patientRepository.InsertAsync(new Patient
            (
                id: Guid.Parse("4fdae80c-b4e0-475b-bdea-fde1fbbfdd86"),
                name: "97d2e118729742259452410e133c394a0f5d2f7be0a640c79236a669bdb2f662feaf801330f144cd9d0188566ab8d1a31d57",
                patientID: "fbc8dfbf24de4ec58b35c2e7f21e4bb219c38a2552b94c479cfd83230e036b836c79ba45bd0f40e6ba352ff0b039c32f3568",
                dateofBirth: new DateTime(2019, 3, 21),
                gender: "e81289e0fcbe4cc5b92ba5330b9620ef95202fa2cfd042cc8d8c71a70f02719008b595ef3e9840619a7413275a4fbc4ad448",
                medicalHistory: "c4e2a9dd7b434e62bcb12146a2e9b708fd813a9d89f5437d92feff9c509f0801c23f6545b6cf4548b4ee4bfd0698cc57e4bd"
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}
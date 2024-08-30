using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Misars.Foundation.App.Patients;
using Misars.Foundation.App.EntityFrameworkCore;
using Xunit;

namespace Misars.Foundation.App.EntityFrameworkCore.Domains.Patients
{
    public class PatientRepositoryTests : AppEntityFrameworkCoreTestBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientRepositoryTests()
        {
            _patientRepository = GetRequiredService<IPatientRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _patientRepository.GetListAsync(
                    name: "06f8dc332c73495f971e62e4f9343801f9f2812825534119a13523d8623c47fac9e75048a4f84a1ca03a78a375856441838a",
                    patientID: "75fa39fb48f141f58dc28764259defd4002da9a66ea1467bbb65c9d3f2f09bc9b938f75f24ca4763a0c3206f876801397fbf",
                    gender: "ae1fe9a9a054437ca3ec8ee64922386f16b683708e2b45a5a1ec44d14899b2b90916b4f5b70d4908bb4c45f872c25d95e661",
                    medicalHistory: "4affb9e6526749e499533a29769981dd60159865815642e8b64cf865bf3c70e31757f71ff66745feb9681a82a3ce240db5f4"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _patientRepository.GetCountAsync(
                    name: "97d2e118729742259452410e133c394a0f5d2f7be0a640c79236a669bdb2f662feaf801330f144cd9d0188566ab8d1a31d57",
                    patientID: "fbc8dfbf24de4ec58b35c2e7f21e4bb219c38a2552b94c479cfd83230e036b836c79ba45bd0f40e6ba352ff0b039c32f3568",
                    gender: "e81289e0fcbe4cc5b92ba5330b9620ef95202fa2cfd042cc8d8c71a70f02719008b595ef3e9840619a7413275a4fbc4ad448",
                    medicalHistory: "c4e2a9dd7b434e62bcb12146a2e9b708fd813a9d89f5437d92feff9c509f0801c23f6545b6cf4548b4ee4bfd0698cc57e4bd"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}
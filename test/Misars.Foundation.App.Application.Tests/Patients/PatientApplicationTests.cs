using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Misars.Foundation.App.Patients
{
    public abstract class PatientsAppServiceTests<TStartupModule> : AppApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IPatientsAppService _patientsAppService;
        private readonly IRepository<Patient, Guid> _patientRepository;

        public PatientsAppServiceTests()
        {
            _patientsAppService = GetRequiredService<IPatientsAppService>();
            _patientRepository = GetRequiredService<IRepository<Patient, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _patientsAppService.GetListAsync(new GetPatientsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("4fdae80c-b4e0-475b-bdea-fde1fbbfdd86")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _patientsAppService.GetAsync(Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new PatientCreateDto
            {
                name = "17ee1a8e785042c1868a2a6295a3b0e429babbaac0624fe5b52d8ece0dcd146bc4b547d0c4ae4f2bb05c942c26013048e266",
                PatientID = "9a291af6f6e24b79949d7c5dccc717bc4fb8d4eded29465c921564970b8fa49003f51d1e5264402d92dac7e612ed45988275",
                DateofBirth = new DateTime(2022, 8, 15),
                Gender = "c19c9ac0c5e94b73934f42adbb4416ca80ed04d0272e4a38a9e4b260d71f9227cd720f22509a44c5b3f4d448ecf04bb8e8d2",
                MedicalHistory = "6e64e1bb096f44138d9ed1a55c3fdab60fb62ef5204643128ee5aaaac3200d9a2cea8637eb6f4483964214bfefe94b7bb0c8"
            };

            // Act
            var serviceResult = await _patientsAppService.CreateAsync(input);

            // Assert
            var result = await _patientRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.name.ShouldBe("17ee1a8e785042c1868a2a6295a3b0e429babbaac0624fe5b52d8ece0dcd146bc4b547d0c4ae4f2bb05c942c26013048e266");
            result.PatientID.ShouldBe("9a291af6f6e24b79949d7c5dccc717bc4fb8d4eded29465c921564970b8fa49003f51d1e5264402d92dac7e612ed45988275");
            result.DateofBirth.ShouldBe(new DateTime(2022, 8, 15));
            result.Gender.ShouldBe("c19c9ac0c5e94b73934f42adbb4416ca80ed04d0272e4a38a9e4b260d71f9227cd720f22509a44c5b3f4d448ecf04bb8e8d2");
            result.MedicalHistory.ShouldBe("6e64e1bb096f44138d9ed1a55c3fdab60fb62ef5204643128ee5aaaac3200d9a2cea8637eb6f4483964214bfefe94b7bb0c8");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new PatientUpdateDto()
            {
                name = "138fbc3ace2e42969fedfbe8a79d02ecd218907383eb422ea1f1b8a2225dd9fbe06b8112e192460ba0ed6ffef6e3d34c85c1",
                PatientID = "f5177a73a69442988332de4b166407d49642a94389db484db99ae693945f15b9629bdd8050464ae7b3e3f44f3a822aea19a8",
                DateofBirth = new DateTime(2019, 2, 3),
                Gender = "9f44a158c2e74eec94926b2ed1ea165f4f4e055488204bc3a2909065413f1ad1fb0af42cd6ae4dd8b733f189ed36c9105557",
                MedicalHistory = "83fc2b9eeefb491b885adbea9e475b7473391ad900e941b393b794f9d129023f2903085d220a453895aa9e83672792bd015d"
            };

            // Act
            var serviceResult = await _patientsAppService.UpdateAsync(Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"), input);

            // Assert
            var result = await _patientRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.name.ShouldBe("138fbc3ace2e42969fedfbe8a79d02ecd218907383eb422ea1f1b8a2225dd9fbe06b8112e192460ba0ed6ffef6e3d34c85c1");
            result.PatientID.ShouldBe("f5177a73a69442988332de4b166407d49642a94389db484db99ae693945f15b9629bdd8050464ae7b3e3f44f3a822aea19a8");
            result.DateofBirth.ShouldBe(new DateTime(2019, 2, 3));
            result.Gender.ShouldBe("9f44a158c2e74eec94926b2ed1ea165f4f4e055488204bc3a2909065413f1ad1fb0af42cd6ae4dd8b733f189ed36c9105557");
            result.MedicalHistory.ShouldBe("83fc2b9eeefb491b885adbea9e475b7473391ad900e941b393b794f9d129023f2903085d220a453895aa9e83672792bd015d");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _patientsAppService.DeleteAsync(Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"));

            // Assert
            var result = await _patientRepository.FindAsync(c => c.Id == Guid.Parse("60acb54b-0451-4e0b-ad4a-df3592dce556"));

            result.ShouldBeNull();
        }
    }
}
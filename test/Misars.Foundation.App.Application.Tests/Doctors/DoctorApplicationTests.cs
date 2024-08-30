using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Misars.Foundation.App.Doctors
{
    public abstract class DoctorsAppServiceTests<TStartupModule> : AppApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IDoctorsAppService _doctorsAppService;
        private readonly IRepository<Doctor, Guid> _doctorRepository;

        public DoctorsAppServiceTests()
        {
            _doctorsAppService = GetRequiredService<IDoctorsAppService>();
            _doctorRepository = GetRequiredService<IRepository<Doctor, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _doctorsAppService.GetListAsync(new GetDoctorsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("6078b90e-c0e9-4f60-9bb2-ae4f2d0f5dfa")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _doctorsAppService.GetAsync(Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new DoctorCreateDto
            {
                name = "25c267925d894afdb59be849221604e418157faddf7c49bdbe6c3b84a2c750f16c1aeae671f04235afdeedbb813b22d4cbf5",
                DoctorID = "8eb279d0a5144848b3c142cc53109c57f26203cea75b499fae736ecfb7e6f16363f98f681c03476b87da7ac4c4ca59c65300",
                Specialty = "426370843cf848c1933ae34605f8af9185c20b3764d84be3997a700c228234acc6e11d3212a54d0b876b9c9c8847dc2d1f95",
                Department = "355e1be43c4d4efc8e351c0e9d52c9e381b22f9f18be4a2ca4ef3ec427a5e0565490dddb3233463885926b22b611235e2944"
            };

            // Act
            var serviceResult = await _doctorsAppService.CreateAsync(input);

            // Assert
            var result = await _doctorRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.name.ShouldBe("25c267925d894afdb59be849221604e418157faddf7c49bdbe6c3b84a2c750f16c1aeae671f04235afdeedbb813b22d4cbf5");
            result.DoctorID.ShouldBe("8eb279d0a5144848b3c142cc53109c57f26203cea75b499fae736ecfb7e6f16363f98f681c03476b87da7ac4c4ca59c65300");
            result.Specialty.ShouldBe("426370843cf848c1933ae34605f8af9185c20b3764d84be3997a700c228234acc6e11d3212a54d0b876b9c9c8847dc2d1f95");
            result.Department.ShouldBe("355e1be43c4d4efc8e351c0e9d52c9e381b22f9f18be4a2ca4ef3ec427a5e0565490dddb3233463885926b22b611235e2944");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new DoctorUpdateDto()
            {
                name = "2c88a23139764da5aca5065cedc89c58358fc98827b9430fb9ee8a73e3789fadccbdec2b3dce4ab593221da3acbdde227705",
                DoctorID = "4274cc11d9f64cf0acc4359e8feccbd6e90646f8cb2040c195417037403e33c370aab1f9dfb54ead95dbe68e70105a8f87ea",
                Specialty = "d432c41de8684725a79926dfeb53f386c1db1524fe7b4f909c168722778dd9727d554103a48e43eea06978870d72c3236d0b",
                Department = "340e088e11e04a229ab02693a1639c584bf83d73794f4bffb1fb397b464a3294697cf6523d48480eb8ade21023f823951190"
            };

            // Act
            var serviceResult = await _doctorsAppService.UpdateAsync(Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b"), input);

            // Assert
            var result = await _doctorRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.name.ShouldBe("2c88a23139764da5aca5065cedc89c58358fc98827b9430fb9ee8a73e3789fadccbdec2b3dce4ab593221da3acbdde227705");
            result.DoctorID.ShouldBe("4274cc11d9f64cf0acc4359e8feccbd6e90646f8cb2040c195417037403e33c370aab1f9dfb54ead95dbe68e70105a8f87ea");
            result.Specialty.ShouldBe("d432c41de8684725a79926dfeb53f386c1db1524fe7b4f909c168722778dd9727d554103a48e43eea06978870d72c3236d0b");
            result.Department.ShouldBe("340e088e11e04a229ab02693a1639c584bf83d73794f4bffb1fb397b464a3294697cf6523d48480eb8ade21023f823951190");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _doctorsAppService.DeleteAsync(Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b"));

            // Assert
            var result = await _doctorRepository.FindAsync(c => c.Id == Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b"));

            result.ShouldBeNull();
        }
    }
}
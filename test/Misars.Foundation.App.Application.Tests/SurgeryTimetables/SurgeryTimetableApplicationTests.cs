using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public abstract class SurgeryTimetablesAppServiceTests<TStartupModule> : AppApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ISurgeryTimetablesAppService _surgeryTimetablesAppService;
        private readonly IRepository<SurgeryTimetable, Guid> _surgeryTimetableRepository;

        public SurgeryTimetablesAppServiceTests()
        {
            _surgeryTimetablesAppService = GetRequiredService<ISurgeryTimetablesAppService>();
            _surgeryTimetableRepository = GetRequiredService<IRepository<SurgeryTimetable, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _surgeryTimetablesAppService.GetListAsync(new GetSurgeryTimetablesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.SurgeryTimetable.Id == Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d")).ShouldBe(true);
            result.Items.Any(x => x.SurgeryTimetable.Id == Guid.Parse("449d6e82-0ffe-4863-b83c-9e37735f1171")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _surgeryTimetablesAppService.GetAsync(Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new SurgeryTimetableCreateDto
            {
                startdate = new DateTime(2017, 4, 23),
                enddate = new DateTime(2023, 6, 3),
                Time = "73f20d4886614ec68c1cc5d7246ba7577f3a1b7905cd4f2aaf23b51423083f42b16c49bf943c4162953cd8b2d443e0c514f1",
                Department = "2b578a2f381f4cca92fc263fc8dd5fd9e3e",
                Diagnosis = "6c94372e934247309ad0223c209f15d8182dc444ac2f437baa6dcec8a4dd98a7d6cb7a9bfa014e6db0d462191f372c27a952",
                SurgicalMethod = "07e8be69d6d0462a975b31b0b56dedc4e1c1e3dbb6fb4dcf87f0d2e6556be4b20f0aae5e47e64b6c9236cd8f10feec31a4c7",
                notes = "028168d617364b68941c5d0aa61fc7e22be8c13c6f914fc3a809b049f4c6e3f29f3d11ba7005405087aa40a484b37b802b30"
            };

            // Act
            var serviceResult = await _surgeryTimetablesAppService.CreateAsync(input);

            // Assert
            var result = await _surgeryTimetableRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.startdate.ShouldBe(new DateTime(2017, 4, 23));
            result.enddate.ShouldBe(new DateTime(2023, 6, 3));
            result.Time.ShouldBe("73f20d4886614ec68c1cc5d7246ba7577f3a1b7905cd4f2aaf23b51423083f42b16c49bf943c4162953cd8b2d443e0c514f1");
            result.Department.ShouldBe("2b578a2f381f4cca92fc263fc8dd5fd9e3e");
            result.Diagnosis.ShouldBe("6c94372e934247309ad0223c209f15d8182dc444ac2f437baa6dcec8a4dd98a7d6cb7a9bfa014e6db0d462191f372c27a952");
            result.SurgicalMethod.ShouldBe("07e8be69d6d0462a975b31b0b56dedc4e1c1e3dbb6fb4dcf87f0d2e6556be4b20f0aae5e47e64b6c9236cd8f10feec31a4c7");
            result.notes.ShouldBe("028168d617364b68941c5d0aa61fc7e22be8c13c6f914fc3a809b049f4c6e3f29f3d11ba7005405087aa40a484b37b802b30");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new SurgeryTimetableUpdateDto()
            {
                startdate = new DateTime(2001, 9, 25),
                enddate = new DateTime(2015, 2, 21),
                Time = "7065d0874c354c1ea216aa518222f11e6446f59656114793a03f0e567283ca63f3b0275438b3409a922614c11df08107a053",
                Department = "49e54048d14b4d48b3286d6b19843ca9a29e87987e7c4bcb82827bf2f755b33bac4865",
                Diagnosis = "9fcdce9a5b8a4f23a2b7aa365372e63553881776efd04401922bc6b5ea526edb873b3f04dfb944728000537deb2bd934635c",
                SurgicalMethod = "377dd50f115d481e868c1d3e3940d1daf387e1fa961c4a70a4c332eca177e77db8aeeb2ed1804267938e1f9d3e6a87a47dd1",
                notes = "29875123e96b4cee9b5b8e490773a7fb4583d8532e284853a86dbb4caed3aa02cf0c5f1eaa9a41a3993e943040a67a965755"
            };

            // Act
            var serviceResult = await _surgeryTimetablesAppService.UpdateAsync(Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d"), input);

            // Assert
            var result = await _surgeryTimetableRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.startdate.ShouldBe(new DateTime(2001, 9, 25));
            result.enddate.ShouldBe(new DateTime(2015, 2, 21));
            result.Time.ShouldBe("7065d0874c354c1ea216aa518222f11e6446f59656114793a03f0e567283ca63f3b0275438b3409a922614c11df08107a053");
            result.Department.ShouldBe("49e54048d14b4d48b3286d6b19843ca9a29e87987e7c4bcb82827bf2f755b33bac4865");
            result.Diagnosis.ShouldBe("9fcdce9a5b8a4f23a2b7aa365372e63553881776efd04401922bc6b5ea526edb873b3f04dfb944728000537deb2bd934635c");
            result.SurgicalMethod.ShouldBe("377dd50f115d481e868c1d3e3940d1daf387e1fa961c4a70a4c332eca177e77db8aeeb2ed1804267938e1f9d3e6a87a47dd1");
            result.notes.ShouldBe("29875123e96b4cee9b5b8e490773a7fb4583d8532e284853a86dbb4caed3aa02cf0c5f1eaa9a41a3993e943040a67a965755");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _surgeryTimetablesAppService.DeleteAsync(Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d"));

            // Assert
            var result = await _surgeryTimetableRepository.FindAsync(c => c.Id == Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d"));

            result.ShouldBeNull();
        }
    }
}
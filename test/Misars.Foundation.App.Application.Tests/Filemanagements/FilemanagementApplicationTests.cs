using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Misars.Foundation.App.Filemanagements
{
    public abstract class FilemanagementsAppServiceTests<TStartupModule> : AppApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IFilemanagementsAppService _filemanagementsAppService;
        private readonly IRepository<Filemanagement, Guid> _filemanagementRepository;

        public FilemanagementsAppServiceTests()
        {
            _filemanagementsAppService = GetRequiredService<IFilemanagementsAppService>();
            _filemanagementRepository = GetRequiredService<IRepository<Filemanagement, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _filemanagementsAppService.GetListAsync(new GetFilemanagementsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("d84f3657-86c0-460c-9278-49626dc70dce")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _filemanagementsAppService.GetAsync(Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new FilemanagementCreateDto
            {
                FileName = "78acccb0e68b46aba4ef3ec71ecf452507130a4b7b9943bf9e65131c79d3b5f493a9b27561924fb393535b0ed64c8d94189b",
                FilePath = "a338f5fef5fa478292c9f35ac97cee692d2909fa4e2c4861968c1b8ef4a822f949f5aa4872214e24961181dbb31ac9e4c007",
                UploadDate = new DateTime(2016, 6, 21)
            };

            // Act
            var serviceResult = await _filemanagementsAppService.CreateAsync(input);

            // Assert
            var result = await _filemanagementRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FileName.ShouldBe("78acccb0e68b46aba4ef3ec71ecf452507130a4b7b9943bf9e65131c79d3b5f493a9b27561924fb393535b0ed64c8d94189b");
            result.FilePath.ShouldBe("a338f5fef5fa478292c9f35ac97cee692d2909fa4e2c4861968c1b8ef4a822f949f5aa4872214e24961181dbb31ac9e4c007");
            result.UploadDate.ShouldBe(new DateTime(2016, 6, 21));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new FilemanagementUpdateDto()
            {
                FileName = "8d4923e9a94f41aaa0f70774df6f1c80975d0f54524749babed7d2bbeded3a656f2c885e96b64750a60179d1fa76118a5ddd",
                FilePath = "e2698c62fc604d26acc01dbfb9f17954177eeef43c8141458e29c81bee04b9af6270f3dcf79b4765b4344b2e1bac086aaa10",
                UploadDate = new DateTime(2023, 7, 13)
            };

            // Act
            var serviceResult = await _filemanagementsAppService.UpdateAsync(Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd"), input);

            // Assert
            var result = await _filemanagementRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FileName.ShouldBe("8d4923e9a94f41aaa0f70774df6f1c80975d0f54524749babed7d2bbeded3a656f2c885e96b64750a60179d1fa76118a5ddd");
            result.FilePath.ShouldBe("e2698c62fc604d26acc01dbfb9f17954177eeef43c8141458e29c81bee04b9af6270f3dcf79b4765b4344b2e1bac086aaa10");
            result.UploadDate.ShouldBe(new DateTime(2023, 7, 13));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _filemanagementsAppService.DeleteAsync(Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd"));

            // Assert
            var result = await _filemanagementRepository.FindAsync(c => c.Id == Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd"));

            result.ShouldBeNull();
        }
    }
}
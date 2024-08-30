using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Misars.Foundation.App.Doctors;
using Misars.Foundation.App.EntityFrameworkCore;
using Xunit;

namespace Misars.Foundation.App.EntityFrameworkCore.Domains.Doctors
{
    public class DoctorRepositoryTests : AppEntityFrameworkCoreTestBase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorRepositoryTests()
        {
            _doctorRepository = GetRequiredService<IDoctorRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _doctorRepository.GetListAsync(
                    name: "46ce3f20c04e4445a10f1b634c47c1ec1b17ebc6722d4bbaa9798ffce730ed3de4f054a5b0e741ccb263e5a393da0ceb8a30",
                    doctorID: "57a662934c61439ea4b544dc37fd76d2f6487cabb3004536882ff1b4a4c5ee484008f80310a64819a96f17f397796e475ef8",
                    specialty: "b62c84f5c4c441118291983f26ab786b0acbe4b0d2a2447e87ddafabb9945672e274416c334e4cd2afd3d032bc307f6e690a",
                    department: "615534d9298b44438e29e8e379624f03bdfdfc2146fa46b7b9af9837e957c42ea0a307bd07af48ac81afd29f59bf0c32d7d5"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("9f20cd05-d9d0-41b7-9b02-c91766f5a01b"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _doctorRepository.GetCountAsync(
                    name: "47d93fd5bd374317be520103e726b75a5044e2c8b5ab413ca82a7694ad48a08c603fd3120f7745bc88fd103cca9ce6452682",
                    doctorID: "a4f9b29fb2e74b14babf78986183c5155608d99283d140fa8ed73d4f4be771dce52fe4ec09f8499b9c6cf8f2da4e09bdf0c6",
                    specialty: "96a833aae3e848c78678d2df82faa17290afe9c909d94f3ea66a478732d024183623c76bde9949c48b6221b30a7d988ed88a",
                    department: "ba4595a28cc945ea92c40822385be7e4a64a50311ddf41d2a73b0445c45d8da6d7810f21ea2a409b993d28071bed98143a05"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}
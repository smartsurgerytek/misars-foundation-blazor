using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Misars.Foundation.App.Filemanagements;

namespace Misars.Foundation.App.Filemanagements
{
    public class FilemanagementsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IFilemanagementRepository _filemanagementRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FilemanagementsDataSeedContributor(IFilemanagementRepository filemanagementRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _filemanagementRepository = filemanagementRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _filemanagementRepository.InsertAsync(new Filemanagement
            (
                id: Guid.Parse("d75e3345-466a-4878-b541-ecaf27f671cd"),
                fileName: "f11952266943467eaf0a812bc555e99577324c7806b744e78f9127f926ee261e3c415264fe3a4699ace94eb5084374372c8f",
                filePath: "b9e7f51aaf6148e9a6b5ebb20961b13f8e1e7c2208e046c69a1f11e58988db9c6a763316b547487896b16238a97a937f5220",
                uploadDate: new DateTime(2021, 1, 16)
            ));

            await _filemanagementRepository.InsertAsync(new Filemanagement
            (
                id: Guid.Parse("d84f3657-86c0-460c-9278-49626dc70dce"),
                fileName: "fa6cd689b90b4dc09efbd19f75b59cabd3ebc7e49601416cb53f67ca6a101ea4ce58f9a84a524aa28a46a92cb019b7cfa99a",
                filePath: "ea6bf924b5a64cfcbf480b80b3f6c75cf7ac2cc1ff9941d097ffe53a93f4efc8c552a21d13ad4624a3108e15070854c86bdf",
                uploadDate: new DateTime(2005, 4, 27)
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}
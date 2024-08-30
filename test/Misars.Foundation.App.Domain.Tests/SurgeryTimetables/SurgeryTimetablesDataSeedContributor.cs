using Misars.Foundation.App.Patients;
using Misars.Foundation.App.Doctors;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Misars.Foundation.App.SurgeryTimetables;

namespace Misars.Foundation.App.SurgeryTimetables
{
    public class SurgeryTimetablesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly ISurgeryTimetableRepository _surgeryTimetableRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly DoctorsDataSeedContributor _doctorsDataSeedContributor;

        private readonly PatientsDataSeedContributor _patientsDataSeedContributor;

        public SurgeryTimetablesDataSeedContributor(ISurgeryTimetableRepository surgeryTimetableRepository, IUnitOfWorkManager unitOfWorkManager, DoctorsDataSeedContributor doctorsDataSeedContributor, PatientsDataSeedContributor patientsDataSeedContributor)
        {
            _surgeryTimetableRepository = surgeryTimetableRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _doctorsDataSeedContributor = doctorsDataSeedContributor; _patientsDataSeedContributor = patientsDataSeedContributor;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _doctorsDataSeedContributor.SeedAsync(context);
            await _patientsDataSeedContributor.SeedAsync(context);

            await _surgeryTimetableRepository.InsertAsync(new SurgeryTimetable
            (
                id: Guid.Parse("84eba765-e720-4ca6-b86b-d8094cab6a1d"),
                startdate: new DateTime(2014, 11, 23),
                enddate: new DateTime(2020, 10, 19),
                time: "a9b6d3b3f22f4a53ba9fbec546e7250bfe49ec12a2ef4aab848302c3ebd26b5996f7fb05f0084f5296f44ee19a29252797f5",
                department: "4a9d43d29a344c5",
                diagnosis: "3fe5b1028d9542c6978ebc3aa1f6cd956ed49aa987ff48c9a621ace2d225cd8308698a56e6414447b0cf6cacfce7ac32fac0",
                surgicalMethod: "34b40fd7663646f0be2ec828973a417a8f36a50c0dee4e99a514c7c4982e7052d799fc7bacf448f2b0e424b4b9537b323359",
                notes: "26ecbaad4c4a4613aa6f6bbcb7fe1b6c25ffcac5d4b94ff6b7108eb572946367859e5f2c731a4fc7afc82330c3822b0f4121",
                doctorId: null,
                patientId: null
            ));

            await _surgeryTimetableRepository.InsertAsync(new SurgeryTimetable
            (
                id: Guid.Parse("449d6e82-0ffe-4863-b83c-9e37735f1171"),
                startdate: new DateTime(2011, 9, 14),
                enddate: new DateTime(2017, 4, 16),
                time: "d36779ab46564ef8be719cfd760a70b13d5e859a363c4869aa48c5ec71f83e329236450cc0594d0888267fbadaae0071e52f",
                department: "aa8af61d237e4",
                diagnosis: "290c625d218f4f8b9e3beb99237c40b82f2f379fda904f3598e935bc40a05e5732a6c29ce4274774bdcc0d7cddef55f0ed0a",
                surgicalMethod: "532bd8eedd09439cba54d00643c752387850f74e3aa444f2bdfd7d8a43fcc8e6be474ef1a84547ca81bba4e114c40a5e0679",
                notes: "9a4a57aede954afb825affc237523ee4809cdb75e3ed49928c6284771dd698cbebb943d681cb4494be9dfe4f77142ad1eaf0",
                doctorId: null,
                patientId: null
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}
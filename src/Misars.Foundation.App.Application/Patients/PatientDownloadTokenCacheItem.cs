using System;

namespace Misars.Foundation.App.Patients;

public abstract class PatientDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}
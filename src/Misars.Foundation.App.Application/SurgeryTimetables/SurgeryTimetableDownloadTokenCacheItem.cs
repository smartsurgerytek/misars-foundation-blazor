using System;

namespace Misars.Foundation.App.SurgeryTimetables;

public abstract class SurgeryTimetableDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}
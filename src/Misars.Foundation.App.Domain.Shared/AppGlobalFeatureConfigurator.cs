using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace Misars.Foundation.App;

public static class AppGlobalFeatureConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.CmsKit(x => x.EnableAll()); 
            GlobalFeatureManager.Instance.Modules.CmsKitPro(x => x.EnableAll());
           /* You can configure (enable/disable) global features of the used modules here.
            * Please refer to the documentation to learn more about the Global Features System:
            * https://docs.abp.io/en/abp/latest/Global-Features
            */
        });
    }
}

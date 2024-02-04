using Prism.Ioc;

namespace Avalonia.Sample.Service;

[ManySingleton<TestService>(typeof(ITestService), typeof(ITestService2))]
internal class TestService : ITestService, ITestService2
{
}

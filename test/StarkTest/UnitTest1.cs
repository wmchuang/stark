using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace StarkTest;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        _testOutputHelper.WriteLine("123");
    }
}
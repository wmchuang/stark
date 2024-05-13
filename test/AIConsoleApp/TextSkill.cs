using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIModule;

public class TextSkill
{
    [KernelFunction]
    [Description("转换大小写")]
    public string Uppercase(Ceshi ceshi)
    {
        Console.WriteLine("进入到我自己的方法 本地函数");
        return ceshi.Text.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
    }
}


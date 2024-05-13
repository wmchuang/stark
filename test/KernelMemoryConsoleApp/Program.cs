// See https://aka.ms/new-console-template for more information

using Json.More;
using KernelMemoryConsoleApp;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;

var handler = new OpenAIHttpClientHandler();
var httpclient = new HttpClient(handler);

var memory = new KernelMemoryBuilder()
    .WithSimpleVectorDb(new SimpleVectorDbConfig()
    {
        Directory = "aaa",
        StorageType = FileSystemTypes.Disk
    })
    .WithOpenAITextGeneration(new OpenAIConfig()
    {
        APIKey = OpenAIOption.ChatToken,
        TextModel = OpenAIOption.ChatModel
    }, null)
    .WithOpenAITextEmbeddingGeneration(new OpenAIConfig()
    {
        // 如果 EmbeddingToken 为空，则使用 ChatToken
        APIKey = string.IsNullOrEmpty(OpenAIOption.EmbeddingToken)
            ? OpenAIOption.ChatToken
            : OpenAIOption.EmbeddingToken,
        EmbeddingModel = OpenAIOption.EmbeddingModel,
    }, null, false)
    .Build<MemoryServerless>();

// 导入网页
{
    // await memory.ImportWebPageAsync(
    //     "https://baike.baidu.com/item/比特币挖矿机/12536531",
    //     documentId: "doc02");
    //
    // Console.WriteLine("正在处理文档，请稍等...");
    // while (!await memory.IsDocumentReadyAsync(documentId: "doc02"))
    // {
    //     await Task.Delay(TimeSpan.FromMilliseconds(1500));
    // }
    //
    // var answer = await memory.AskAsync("今天赚了多少钱？");
    // Console.WriteLine($"\nAnswer: {answer.Result}");
}
//导入文本
{
    // await memory.ImportTextAsync("今天赚了120元", documentId: "doc02");
    //
    // var answer = await memory.AskAsync("今天赚了多少钱？" );
    // Console.WriteLine($"\nAnswer: {answer.Result}");
}

//导入文件
{
    string documentId = Guid.NewGuid().ToString();
     // await memory.ImportDocumentAsync("gs.txt", documentId: documentId, index: "document");

    //  await memory.DeleteDocumentAsync(documentId: "37843e4e-6e69-4d1e-b69b-09941c42b28e", index: "document");
    // var answer = await memory.AskAsync("郑州易宝在哪？", index: "document");
    // Console.WriteLine($"\nAnswer: {answer.Result}");
    
    
    var xlresult = await memory.SearchAsync("郑州易宝在哪？", index: "document");
    Console.WriteLine("搜索结果：");
    if (xlresult != null)
    {
        
        foreach (var item in xlresult.Results)
        {
            foreach (var part in item.Partitions)
            {
                Console.WriteLine(
                    $"DocumentId: {    item.ToJsonDocument()}; [file:{item.SourceName};    相关性:{(part.Relevance * 100).ToString("F2")}%]:{part.Text}");
                Console.WriteLine("----------------------");
            }
        }
    }
    
}

Console.WriteLine("Hello, World!");
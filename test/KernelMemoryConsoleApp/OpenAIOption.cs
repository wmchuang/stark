namespace KernelMemoryConsoleApp;

public class OpenAIOption
{
    public const string Name = "OpenAI";

    /// <summary>
    /// 对话模型的 API Endpoint
    /// </summary>
    public static string ChatEndpoint { get; set; } = "http://101.133.236.73:13308";

    /// <summary>
    /// 量化模型的 API Endpoint
    /// </summary>
    public static string EmbeddingEndpoint { get; set; } = "http://101.133.236.73:13308";

    /// <summary>
    /// 对话模型的 API Key
    /// </summary>
    public static string ChatToken { get; set; } = "";

    /// <summary>
    /// 量化模型的 API Key
    /// </summary>
    public static string EmbeddingToken { get; set; } = "";

    /// <summary>
    /// 对话模型
    /// </summary>
    // public static string ChatModel { get; set; } = "ERNIE-Bot";
    //
    // /// <summary>
    // /// 量化模型
    // /// </summary>
    // public static string EmbeddingModel { get; set; } = "text-embedding-v1";
    
    /// <summary>
    /// 对话模型
    /// </summary>
    public static string ChatModel { get; set; } = "gpt-3.5-turbo";

    /// <summary>
    /// 量化模型
    /// </summary>
    public static string EmbeddingModel { get; set; } = "text-embedding-3-small";
}
namespace Stark.Module.System.Models.Results.Auth;

public class GetCaptchaResult
{
    public string CaptchaKey { get; set; }

    public string CaptchaBase64 { get; set; }
}
namespace Application.Options;

public class EmailOptions
{
    public const string SectionName = "Email";

    public SmtpSettings Smtp { get; set; } = new();
    public SenderSettings Sender { get; set; } = new();
    public string ConfirmationBaseUrl { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class SenderSettings
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

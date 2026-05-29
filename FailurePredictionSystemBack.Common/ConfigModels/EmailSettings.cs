namespace FailurePredictionSystemBack.Common.ConfigModels;

public class EmailSettings
{
    public bool Enabled { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public bool UseSsl { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
}
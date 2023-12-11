namespace PecqBoxingClubApi.BackEnd.Configuration;

public class GlobalSettings
{
    public virtual string ProjectName { get; set; }
    public virtual string LogDirectory { get; set; }
    public virtual string FileDirectory { get; set; }
    public virtual int MinutesForVoting { get; set; }
    public virtual string Question { get; set; }
    public virtual SqlServerSettings SqlServer { get; set; } = new SqlServerSettings();
    public virtual BaseServiceUriSettings BaseServiceUri { get; set; } = new BaseServiceUriSettings();
    public virtual IdentityServerSettings IdentityServer { get; set; } = new IdentityServerSettings();
    public virtual IdentitySettings Identity { get; set; } = new IdentitySettings();
    public virtual MailSettings Mail { get; set; } = new MailSettings();
    public virtual WebSiteUriSettings WebSiteUri { get; set; } = new WebSiteUriSettings();
    public virtual SwaggerSettings Swagger { get; set; } = new SwaggerSettings();
    public virtual HangfireSettings Hangfire { get; set; } = new HangfireSettings();
    public virtual OpenVPNSettings OpenVPN { get; set; } = new OpenVPNSettings();
    public virtual ApiKeySettings ApiKey { get; set; } = new ApiKeySettings();
    public virtual RamDamApiSettings RamDamApi { get; set; } = new RamDamApiSettings();

    public class SqlServerSettings
    {
        private string _connectionString;

        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value.Trim('"');
        }
    }

    public class BaseServiceUriSettings
    {
        public string Api { get; set; }
        public string Identity { get; set; }
        public string InternalIdentity { get; set; }
    }

    public class RamDamApiSettings
    {
        public string ApiUrl { get; set; }
        public bool IsStandBy { get; set; }
        public string StandByMessage { get; set; }
    }

    public class IdentityServerSettings
    {
        public string CertificatePassword { get; set; }
    }

    public class IdentitySettings
    {
        public uint? DataProtectorTokenLifeSpan { get; set; }
    }

    public class MailSettings
    {
        public SmtpSettings Smtp { get; set; } = new SmtpSettings();

        public string SenderEmail { get; set; }
        public string ReplyEmail { get; set; }
        public string SenderName { get; set; }
        public Dictionary<string, string> Subjects { get; set; }

        public class SmtpSettings
        {
            public string Host { get; set; }
            public uint Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }

    public class WebSiteUriSettings
    {
        private string _welcome;
        private string _changePassword;
        private string _dispatcherConfirm;
        private string _dispatcherConfirmComment;
        private string _driverConfirm;
        private string _driverConfirmComment;

        public string Base { get; set; }
        public string Welcome
        {
            get => $"{Base}/{_welcome}";
            set => _welcome = value;
        }
        public string ChangePassword
        {
            get => $"{Base}/{_changePassword}";
            set => _changePassword = value;
        }
        public string DispatcherConfirm
        {
            get => $"{Base}/{_dispatcherConfirm}";
            set => _dispatcherConfirm = value;
        }
        public string DispatcherConfirmComment
        {
            get => $"{Base}/{_dispatcherConfirmComment}";
            set => _dispatcherConfirmComment = value;
        }
        public string DriverConfirm
        {
            get => $"{Base}/{_driverConfirm}";
            set => _driverConfirm = value;
        }
        public string DriverConfirmComment
        {
            get => $"{Base}/{_driverConfirmComment}";
            set => _driverConfirmComment = value;
        }
    }

    public class SwaggerSettings
    {
        public bool IsActive { get; set; }
    }

    public class HangfireSettings
    {
        public RecurringJobsSettings RecurringJobs { get; set; } = new RecurringJobsSettings();

        public class RecurringJobsSettings
        {
            public string SyncLogs { get; set; }
        }
    }

    public class OpenVPNSettings
    {
        public string ProcessFilepath { get; set; }
        public string ConfigFilepath { get; set; }
        public string LogFilename { get; set; }
        public string CredsFilepath { get; set; }
        public string WorkingDirectory { get; set; }
    }

    public class ApiKeySettings
    {
        public string Smartwaste { get; set; }
    }
}

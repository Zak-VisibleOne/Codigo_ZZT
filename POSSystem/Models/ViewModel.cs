using System;
namespace POSSystem.Models
{
    public class ViewModel
    {
        public class ConfigModel
        {
            public int ID { get; set; }
            public string SiteUrl { get; set; }
            public string FrontSiteUrl { get; set; }
            public string SiteName { get; set; }
            public string SiteMetaDescription { get; set; }
            public string SiteMetaKeywords { get; set; }
            public Nullable<bool> ShowAuthorMetaTag { get; set; }
            public string DatabaseType { get; set; }
            public string DatabaseUsername { get; set; }
            public string DatabaseName { get; set; }
            public Nullable<bool> SendMail { get; set; }
            public string FromEmail { get; set; }
            public string ToEmail { get; set; }
            public string ToEmailContact { get; set; }
            public string ToEmailSubscribeUser { get; set; }
            public string ccToEmail { get; set; }
            public string MailSubjects { get; set; }
            public string MailSubjectsContact { get; set; }
            public string MailSubjectSubscribe { get; set; }
            public string MailBody { get; set; }
            public string MailBodyContact { get; set; }
            public string OldApiUrl { get; set; }
            public string NewApiUrl { get; set; }
            public string ApiKey { get; set; }
            public string ChannelID { get; set; }
            public string MailBodySubscribe { get; set; }
        }
        public class VerifyEmailVM
        {
            public int ID { get; set; }
            public string Subject_enUS { get; set; }
            public string Subject_zhHK { get; set; }
            public string Subject_zhCN { get; set; }
            public string Body_enUS { get; set; }
            public string Body_zhHK { get; set; }
            public string Body_zhCN { get; set; }
            public string Method { get; set; }
            public string Remark { get; set; }
            public string Server { get; set; }
            public string Port { get; set; }
            public string SenderMail { get; set; }
            public string MailPassword { get; set; }
        }
        public class UserCential
        {
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }
        public class RMail
        {
            public int ID { get; set; }
            public string Email { get; set; }
            public string Method { get; set; }
        }
        public class ViewModelColun
        {
            public int count { get; set; }
            public string TABLE_NAME { get; set; }
            public string COLUMN_NAME { get; set; }
        }
        public class ViewModelCategory
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public Nullable<bool> DeleteStatus { get; set; }
            public Nullable<int> AccessID { get; set; }
            public string AccessName { get; set; }
            public Nullable<int> LanguageID { get; set; }
            public string LanguageTitle { get; set; }
            public string Method { get; set; }
        }
        public class ViewModelUserInfo
        {
            public Int64 SrNo { get; set; }
            public int Id { get; set; }
            public string UserCode { get; set; }
            public string UserName { get; set; }
            public string UserName_zhHK { get; set; }
            public string UserName_zhCN { get; set; }
            public string ShortDesc_enUS { get; set; }
            public string ShortDesc_zhHk { get; set; }
            public string ShortDesc_zhCN { get; set; }
            public string Description_enUS { get; set; }
            public string Description_zhHK { get; set; }
            public string Description_zhCN { get; set; }
            public string ImageUrl { get; set; }
            public string DefaultCompany { get; set; }
            public string Password { get; set; }
            public string UserGroupCode { get; set; }
            public Nullable<int> RoleID { get; set; }
            public string Email { get; set; }
            public Nullable<System.DateTime> LastLogin { get; set; }
            public int IsLogin { get; set; }
            public Nullable<int> IsLoginLimit { get; set; }
            public Nullable<bool> DeletedStatus { get; set; }
        }
        public class ViewModelUserGroup
        {
            public Int64 SrNo { get; set; }
            public string GroupCode { get; set; }
            public string GroupName { get; set; }
            public int ID { get; set; }
        }
        public class ViewModelLog
        {
            public Int64 SrNo { get; set; }
            public string LogDate { get; set; }
            public string CallerName { get; set; }
            public string LogAction { get; set; }
        }
        public class ViewModelMedia
        {
            public int ID { get; set; }
            public string MediaName_enUS { get; set; }
            public string MediaUrl_enUS { get; set; }
            public string MediaTypeName { get; set; }
            public string Size { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedUser { get; set; }
            public string Method { get; set; }
        }

        public partial class viewExport
        {
            public int SrNo { get; set; }
            public string Name { get; set; }
        }
    }
}
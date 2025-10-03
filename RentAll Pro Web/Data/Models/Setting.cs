using System.ComponentModel.DataAnnotations;

namespace RentAll_Pro_Web.Data.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }
        public string? CompanyLogo { get; set; }

        [Required]
        public string EmailSmtp { get; set; }
        public int SmtpPort { get; set; } = 587;

        [Required]
        public string EmailPassword { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string SenderEmail { get; set; }
        public string? CcAddress { get; set; }

        [Required]
        public string EmailSubject { get; set; }
        public string? ContractEmailTemplate { get; set; }

        [Required]
        public string ReviewEmailSubject { get; set; }
        public string? ReviewEmailTemplate { get; set; }

        public string? TemplateContract { get; set; }
        public string? AszfFile { get; set; }
        public string? GoogleReview { get; set; }
        public string? InvoiceXml { get; set; }

        public int DefaultRentalDays { get; set; } = 1;
    }
}
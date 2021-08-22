namespace Cloud.Core.Notification
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>Interface for Email notification providers.</summary>
    public interface IEmailProvider : INamedInstance
    {
        /// <summary>Sent an email synchronously.</summary>
        /// <param name="email">The email to send.</param>
        /// <returns><c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        EmailSendResult Send(EmailMessage email);

        /// <summary>Sends an email synchronously.</summary>
        /// <param name="templatedEmail">The templated email.</param>
        /// <returns><c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        EmailSendResult Send(EmailTemplateMessage templatedEmail);

        /// <summary>Sends an email asynchronously.</summary>
        /// <param name="email">The email to send.</param>
        /// <returns>Task&lt;System.Boolean&gt;. <c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        Task<EmailSendResult> SendAsync(EmailMessage email);

        /// <summary>Sends an email asynchronously.</summary>
        /// <param name="templatedEmail">The templated email to send.</param>
        /// <returns><c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        Task<EmailSendResult> SendAsync(EmailTemplateMessage templatedEmail);
    }

    /// <summary>Email send result.</summary>
    public class EmailSendResult
    {
        /// <summary>Success [true] or failure [false].</summary>
        public bool Success { get; set; }

        /// <summary>Error message if failed to send.</summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>Email message.</summary>
    public class EmailMessage
    {
        /// <summary>List of email recipient (each sent as blind carbon copy).</summary>
        public List<string> To { get; } = new List<string>();

        /// <summary>Email subject.</summary>
        public string Subject { get; set; }

        /// <summary>Gets or sets the email content.</summary>
        public string Content { get; set; }

        /// <summary>The email attachments.</summary>
        public List<EmailAttachment> Attachments { get; } = new List<EmailAttachment>();
    }

    /// <summary>
    /// Email message using a pre-existing template.
    /// </summary>
    public class EmailTemplateMessage
    {
        /// <summary>List of email recipient (each sent as blind carbon copy).</summary>
        public List<string> To { get; } = new List<string>();

        /// <summary>Email subject.</summary>
        public string Subject { get; set; }

        /// <summary>The email template.</summary>
        public string TemplateId { get; set; }

        /// <summary>The object to map to the email template.</summary>
        public object TemplateObject { get; set; }

        /// <summary>The email attachments.</summary>
        public List<EmailAttachment> Attachments { get; } = new List<EmailAttachment>();
    }

    /// <summary>Email attachment.</summary>
    public class EmailAttachment
    {
        /// <summary>Attachment file name.</summary>
        public string Name { get; set; }

        /// <summary>Attachment content type, example 'application/pdf'.</summary>
        public string ContentType { get; set; }

        /// <summary>Content of the attachment.</summary>
        public Stream Content { get; set; }
    }
}

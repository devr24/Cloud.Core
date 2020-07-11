namespace Cloud.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>Interface for Email notification providers.</summary>
    public interface IEmailProvider : INamedInstance
    {
        /// <summary>Sent an email synchronously.</summary>
        /// <param name="email">The email to send.</param>
        /// <returns><c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        bool Send(IEmailMessage email);

        /// <summary>Sends an email asynchronously.</summary>
        /// <param name="email">The email to send.</param>
        /// <returns>Task&lt;System.Boolean&gt;. <c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        Task<bool> SendAsync(IEmailMessage email);
    }

    /// <summary>Email message.</summary>
    public interface IEmailMessage
    {
        /// <summary>Gets or sets the recipient list (send as blind carbon copy).</summary>
        List<IEmailRecipient> To { get; }

        /// <summary>Gets or sets the email subject.</summary>
        string Subject { get; set; }

        /// <summary>Gets or sets the name of the email template to use.</summary>
        string TemplateName { get; set; }

        /// <summary>Gets or sets the email content.</summary>
        string Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is plain text.
        /// </summary>
        bool IsPlainText { get; set; }

        /// <summary>Gets or sets the email attachments.</summary>
        List<IEmailAttachment> Attachments { get; }
    }

    /// <summary>Email recipient.</summary>
    public interface IEmailRecipient
    {
        /// <summary>Email recipient name.</summary>
        string Name { get; set; }

        /// <summary>Email recipient address.</summary>
        string Address { get; set; }
    }

    /// <summary>Email attachment.</summary>
    public interface IEmailAttachment
    {
        /// <summary>Attachment file name.</summary>
        string Name { get; set; }

        /// <summary>Attachment content type, example 'application/pdf'.</summary>
        string ContentType { get; set; }

        /// <summary>Content of the attachment.</summary>
        Stream Content { get; set; }
    }
}

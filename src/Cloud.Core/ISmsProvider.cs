namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>Interface for Sms notification providers.</summary>
    public interface ISmsProvider : INamedInstance
    {
        /// <summary>Send sms synchronously.</summary>
        /// <param name="sms">The sms to send.</param>
        /// <returns><c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        bool Send(SmsMessage sms);

        /// <summary>Sends sms asynchronously.</summary>
        /// <param name="sms">The sms to send.</param>
        /// <returns>Task&lt;System.Boolean&gt;. <c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        Task<bool> SendAsync(SmsMessage sms);
    }

    /// <summary>Sms Message.</summary>
    public class SmsMessage
    {
        /// <summary>Recipient list for Sms message.</summary>
        public List<string> To { get; set; }

        /// <summary>Gets or sets the message text.</summary>
        public string Text { get; set; }
        
        // ReSharper Disable All
        /// <summary>Collection of Sms links to attach to the message.</summary>
        public List<SmsLink> Links { get; } = new List<SmsLink>();
        // ReSharper Restore All

        /// <summary>Gets the text plus links together as the "full content" of the message.</summary>
        public string FullContent => $"{Text}{(Links.Any() ? "\n" : string.Empty)}{string.Join("\n", Links.Select(l => l.ToString()))}";
    }

    /// <summary>Sms link containing a title and uri link.</summary>
    public class SmsLink
    {
        /// <summary>Title for the Sms link.</summary>
        [Required]
        public string Title { get; set; }

        /// <summary>Uri for the Sms link.</summary>
        [Required]
        public Uri Link { get; set; }

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Title}: {Link}";
        }
    }
}

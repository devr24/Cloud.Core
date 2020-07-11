namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>Interface for Sms notification providers.</summary>
    public interface ISmsProvider : INamedInstance
    {
        /// <summary>Send sms synchronously.</summary>
        /// <param name="sms">The sms to send.</param>
        /// <returns><c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        bool Send(ISmsMessage sms);

        /// <summary>Sends sms asynchronously.</summary>
        /// <param name="sms">The sms to send.</param>
        /// <returns>Task&lt;System.Boolean&gt;. <c>True</c> if sent successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        Task<bool> SendAsync(ISmsMessage sms);
    }

    /// <summary>Sms Message.</summary>
    public interface ISmsMessage
    {
        /// <summary>Recipient list for Sms message.</summary>
        List<string> To { get; set; }

        /// <summary>Gets or sets the message text.</summary>
        string Text { get; set; }
        
        /// <summary>Collection of Sms links to attach to the message.</summary>
        List<ISmsLink> Links { get; }

        /// <summary>Gets the text plus links together as the "full content" of the message.</summary>
        string FullContent { get; }
    }

    /// <summary>Sms link containing a title and uri link.</summary>
    public interface ISmsLink
    {
        /// <summary>Title for the Sms link.</summary>
        string Title { get; set; }

        /// <summary>Uri for the Sms link.</summary>
        Uri Link { get; set; }
    }
}

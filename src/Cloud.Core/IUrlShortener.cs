namespace Cloud.Core
{
    using System;
    using System.Threading.Tasks;

    /// <summary>Interface for Url shortening service implementations.</summary>
    public interface IUrlShortener
    {
        /// <summary>Shorten the passed in link.</summary>
        /// <param name="original">Original link to shorten.</param>
        /// <returns>Task ILinkShortenResult with the short link.</returns>
        /// <exception cref="Exceptions.RequestFailedException{T}">Error during processing.</exception>
        Task<IUrlShortenResult> ShortenLink(Uri original);
    }

    /// <summary>Interface for shorten result.</summary>
    public interface IUrlShortenResult
    {
        /// <summary>Original source link to shorten.</summary>
        Uri SourceLink { get; set; }

        /// <summary>Shortened version of the link.</summary>
        Uri ShortLink { get; set; }

        /// <summary>Value indicating whether the shortening result <see cref="IUrlShortenResult"/> was a success.</summary>
        bool Success { get; set; }

        /// <summary>Result message - populated if an error occurs</summary>
        string Message { get; set; }
    }
}

namespace Cloud.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Cloud.Core.Exceptions;

    /// <summary>Interface for ITemplateMapper implementations.</summary>
    public interface ITemplateMapper
    {
        /// <summary>Gets the result of the html template lookup.</summary>
        /// <param name="templateId">Name of the template to load contents of.</param>
        /// <returns>Task&lt;TemplateInfo&gt; template information found.</returns>
        Task<ITemplateResult> GetTemplateContent(string templateId);

        /// <summary>
        /// Maps to HTML and retuns the raw html string.
        /// </summary>
        /// <param name="templateId">Name of the template.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapToHtml(string templateId, object model);

        /// <summary>Maps a model into HTML and returns the result as a PDF base64 string.</summary>
        /// <param name="templateId">Name of the template to map to.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapToHtmlAsPdfBase64(string templateId, object model);

        /// <summary>Maps a model into HTML and returns the result as a PDF stream.</summary>
        /// <param name="templateId">Name of the template to map to.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<Stream> MapToHtmlAsPdfStream(string templateId, object model);
    }

    /// <summary>Template lookup result.</summary>
    public interface ITemplateResult
    {
        /// <summary>Name of the template requested.</summary>
        string TemplateId { get; set; }

        /// <summary>Whether the template requested was actually found <c>true</c> or not <c>false</c>.</summary>
        bool TemplateFound { get; set; }

        /// <summary>The list of Keys found on the template.</summary>
        List<string> TemplateKeys { get; set; }

        /// <summary>Content of the template.</summary>
        string TemplateContent { get; set; }
    }
}

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
        /// <param name="templateName">Name of the template to load contents of.</param>
        /// <returns>Task&lt;TemplateInfo&gt; template information found.</returns>
        Task<TemplateResult> GetTemplateContent(string templateName);

        /// <summary>
        /// Maps to HTML and retuns the raw html string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapToHtml<T>(T model, string templateName);

        /// <summary>Maps a model into HTML and returns the result as a PDF base64 string.</summary>
        /// <typeparam name="T">Model to map into the Html.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template to map to.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapToHtmlAsPdfBase64<T>(T model, string templateName);

        /// <summary>Maps a model into HTML and returns the result as a PDF stream.</summary>
        /// <typeparam name="T">Model to map into the Html.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template to map to.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<Stream> MapToHtmlAsPdfStream<T>(T model, string templateName);
    }

    /// <summary>Template lookup result.</summary>
    public class TemplateResult
    {
        /// <summary>Name of the template requested.</summary>
        public string TemplateName { get; set; }

        /// <summary>Whether the template requested was actually found <c>true</c> or not <c>false</c>.</summary>
        public bool TemplateFound { get; set; }

        /// <summary>The list of Keys found on the template.</summary>
        public List<string> TemplateKeys { get; set; }

        /// <summary>Content of the template.</summary>
        public string TemplateContent { get; set; }
    }
}

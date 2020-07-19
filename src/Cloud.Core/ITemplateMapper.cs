namespace Cloud.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Cloud.Core.Exceptions;

    /// <summary>Interface for ITemplateMapper implementations.</summary>
    public interface ITemplateMapper
    {
        /// <summary>Gets the result of the template lookup.</summary>
        /// <param name="templateId">Name of the template to load contents of.</param>
        /// <returns>Task&lt;TemplateInfo&gt; template information found.</returns>
        Task<ITemplateResult> GetTemplateContent(string templateId);

        /// <summary>
        /// Maps to template and retuns the raw template result string.
        /// </summary>
        /// <param name="templateId">Identity of the template to load.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapTemplateUsingId(string templateId, object model);

        /// <summary>
        /// Maps to template and retuns the raw template result string.
        /// </summary>
        /// <param name="templateContent">Content of the template to map.</param>
        /// <param name="model">The model to use during replacement.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapTemplateContent(string templateContent, object model);

        /// <summary>Maps a model into a template and returns the result as a PDF base64 string.</summary>
        /// <param name="templateId">Identity of the template to map to.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapTemplateAsPdfBase64(string templateId, object model);

        /// <summary>Maps a model into a template and returns the result as a PDF stream.</summary>
        /// <param name="templateId">Identity of the template to map to.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<Stream> MapTemplateAsPdfStream(string templateId, object model);
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

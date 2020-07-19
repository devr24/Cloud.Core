namespace Cloud.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Cloud.Core.Exceptions;

    /// <summary>Interface for ITemplateMapper implementations.</summary>
    public interface ITemplateMapper
    {
        /// <summary>Gets the template content using the Id.</summary>
        /// <param name="templateId">Id of the template to load contents for.</param>
        /// <returns>Task&lt;TemplateInfo&gt; template mapping information.</returns>
        Task<ITemplateResult> GetTemplateContent(string templateId);

        /// <summary>
        /// Maps the model to a stored template contents (identified by template Id) and retuns the raw template result string.
        /// </summary>
        /// <param name="templateId">Identity of the template to load.</param>
        /// <param name="model">The model to map to the content.</param>
        /// <returns>Task&lt;TemplateInfo&gt; template mapping information.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<ITemplateResult> MapTemplateId(string templateId, object model);

        /// <summary>
        /// Maps the model to the template content passed in and retuns the raw template result string.
        /// </summary>
        /// <param name="templateContent">Content of the template to map.</param>
        /// <param name="model">The model to map to the content.</param>
        /// <returns>Task&lt;TemplateInfo&gt; template mapping information.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<ITemplateResult> MapTemplateContent(string templateContent, object model);

        /// <summary>Maps the model to the template content and returns the result as a PDF base64 string.</summary>
        /// <param name="templateContent">Content of the template to map to.</param>
        /// <param name="model">The model to map to the content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapTemplateContentAsPdfBase64(string templateContent, object model);

        /// <summary>Maps the model into the template content and returns the result as a PDF stream.</summary>
        /// <param name="templateContent">Content of the template to map to.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<Stream> MapTemplateContentAsPdfStream(string templateContent, object model);

        /// <summary>Maps the model to the template contents (identified by template Id) and returns the result as a PDF base64 string.</summary>
        /// <param name="templateId">Identity of the template to map to.</param>
        /// <param name="model">The model to map to the content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<string> MapTemplateIdAsPdfBase64(string templateId, object model);

        /// <summary>Maps the model to the template contents (identified by template Id) and returns the result as a PDF stream.</summary>
        /// <param name="templateId">Identity of the template to map to.</param>
        /// <param name="model">The model to map to the content.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="TemplateMappingException">Error during template lookup.</exception>
        Task<Stream> MapTemplateIdAsPdfStream(string templateId, object model);
    }

    /// <summary>Template mapping result.</summary>
    public interface ITemplateResult
    {
        /// <summary>The list of Keys found on the template.</summary>
        List<string> TemplateKeys { get; set; }

        /// <summary>Content of the template.</summary>
        string TemplateContent { get; set; }
    }
}

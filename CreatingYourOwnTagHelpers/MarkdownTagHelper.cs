using System;
using System.Linq;
using System.Threading.Tasks;
using CommonMark;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CreatingYourOwnTagHelpers
{
    [HtmlTargetElement(Attributes = "markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        public string Suffix { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var lines = childContent.GetContent()
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim());
            var content = string.Join(" ", lines);
            var transformedContent = CommonMarkConverter.Convert(content);

            output.Content.SetHtmlContent(transformedContent);
            output.PostContent.SetHtmlContent(Suffix);

            output.Attributes.RemoveAll("markdown");
        }
    }
}

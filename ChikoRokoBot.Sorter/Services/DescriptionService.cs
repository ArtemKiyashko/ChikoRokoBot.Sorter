using ChikoRokoBot.Sorter.Extensions;
using ChikoRokoBot.Sorter.Helpers;
using ChikoRokoBot.Sorter.Interfaces;
using Markdig;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;

namespace ChikoRokoBot.Sorter.Services
{
	public class DescriptionService : IDescriptionService
    {
        private readonly Converter _converter;
        private readonly ILogger<DescriptionService> _logger;

        public DescriptionService(Converter converter, ILogger<DescriptionService> logger)
		{
            _converter = converter;
            _logger = logger;
        }

        public string GetMarkdownDescription(string html)
        {
            _logger.LogInformation($"Original description: {html}");

            var markdownDoc = Markdig.Markdown.Parse(_converter.Convert(html));

            foreach (var block in markdownDoc)
            {
                var t = block.Parser;
            }

            var sanitizedDescription = TelegramMarkdownSanitizer.Sanitize(html);

            _logger.LogInformation($"Sanitized description: {sanitizedDescription}");

            var markdown = _converter.Convert(sanitizedDescription);

            _logger.LogInformation($"Markdown description: {markdown}");

            return markdown;
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using RealTimeTranslator.TextToSpeechService;

namespace RealTimeTranslator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextToSpeechController : ControllerBase
    {
        private readonly ILogger<TextToSpeechController> _logger;
        private readonly TextToSpeech _textToSpeech;

        public TextToSpeechController(ILogger<TextToSpeechController> logger, TextToSpeech textToSpeech)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _textToSpeech = textToSpeech ?? throw new ArgumentNullException(nameof(textToSpeech));
        }

        [HttpPost("convert", Name = "Convert")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Post([FromBody] string text)
        {
            var audioBytes = await _textToSpeech.ConvertTextToSpeech(text);
            return File(audioBytes, "audio/wav", "output");
        }
    }
}

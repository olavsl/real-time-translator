using Microsoft.AspNetCore.Mvc;
using TextToSpeech.TextToSpeechService;

namespace TextToSpeech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TTSController : ControllerBase
    {
        private readonly ILogger<TTSController> _logger;
        private readonly TTSConverter _ttsConverter;

        public TTSController(ILogger<TTSController> logger, TTSConverter ttsConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ttsConverter = ttsConverter ?? throw new ArgumentNullException(nameof(ttsConverter));
        }

        [HttpPost("convert", Name = "Convert")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Post([FromBody] string text)
        {
            var audioBytes = await _ttsConverter.ConvertTextToSpeech(text);
            return File(audioBytes, "audio/wav", "output");
        }
    }
}

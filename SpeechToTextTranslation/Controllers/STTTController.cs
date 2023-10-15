using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SpeechToTextTranslation.SpeechToTextTranslationService;

namespace SpeechToTextTranslation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class STTTController : ControllerBase
    {
        private readonly ILogger<STTTController> _logger;
        private readonly STTTConverter _stttConverter;

        public STTTController(ILogger<STTTController> logger, STTTConverter stttConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stttConverter = stttConverter ?? throw new ArgumentNullException(nameof(stttConverter));
        }

        [HttpPost("STTT/convert", Name = "STTT/Convert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Post(IFormFile audioFile)
        {
            string translatedText = await _stttConverter.ConvertAndTranslateSpeechToText(audioFile);
            return Results.Ok(translatedText);
        }
    }
}

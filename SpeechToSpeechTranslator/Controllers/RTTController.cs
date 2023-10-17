using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SpeechToSpeechTranslator.SpeechToSpeechTranslatorService;

namespace SpeechToSpeechTranslator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RTTController : ControllerBase
    {
        private readonly ILogger<RTTController> _logger;
        private readonly RTT _rtt;

        public RTTController(ILogger<RTTController> logger, RTT rtt)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rtt = rtt ?? throw new ArgumentNullException(nameof(rtt));
        }

        [HttpPost("translate", Name = "Translate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(IFormFile audioFile)
        {
            if (audioFile == null)
            {
                return BadRequest();
            }

            return await _rtt.SpeechToSpeechTranslation(audioFile);
        }
    }
}

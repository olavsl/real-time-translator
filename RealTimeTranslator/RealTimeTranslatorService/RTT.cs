using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RealTimeTranslator.Options;
using RealTimeTranslator.SpeechToTextTranslationService;
using RealTimeTranslator.TextToSpeechService;

namespace RealTimeTranslator.RealTimeTranslatorService
{
    public class RTT
    {
        private readonly ILogger<RTT> _logger;
        private readonly HttpClient _httpClient;
        private readonly STTTConverter _stttConverter;
        private readonly TTSConverter _ttsConverter;

        public RTT(ILogger<RTT> logger, STTTConverter stttConverter, TTSConverter ttsConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = new HttpClient();
            _stttConverter = stttConverter ?? throw new ArgumentNullException(nameof(stttConverter));
            _ttsConverter = ttsConverter ?? throw new ArgumentNullException(nameof(ttsConverter));
        }

        public async Task<IActionResult> SpeechToSpeechTranslation(IFormFile audioFile)
        {
            string translatedText = await _stttConverter.ConvertAndTranslateSpeechToText(audioFile);
            _logger.LogInformation($"Translated text: {translatedText}");
            byte[] translatedAudio = await _ttsConverter.ConvertTextToSpeech(translatedText);
            return new FileContentResult(translatedAudio, "audio/wav")
            {
                FileDownloadName = "translatedAudio.wav"
            };
        }
    }
}
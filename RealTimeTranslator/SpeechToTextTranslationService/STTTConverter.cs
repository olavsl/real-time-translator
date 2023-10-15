using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Microsoft.Extensions.Options;
using RealTimeTranslator.Options;

namespace RealTimeTranslator.SpeechToTextTranslationService
{
    public class STTTConverter
    {
        private readonly ILogger<STTTConverter> _logger;
        private readonly IOptionsMonitor<STTTOptions> _stttOptions;

        private readonly SpeechTranslationConfig speechTranslationConfig;

        public STTTConverter(ILogger<STTTConverter> logger, IOptionsMonitor<STTTOptions> stttOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stttOptions = stttOptions ?? throw new ArgumentNullException(nameof(stttOptions));
            speechTranslationConfig = SpeechTranslationConfig.FromSubscription(_stttOptions.CurrentValue.SubscriptionKey, _stttOptions.CurrentValue.Region);
            speechTranslationConfig.SpeechRecognitionLanguage = _stttOptions.CurrentValue.SourceLanguage;
            speechTranslationConfig.AddTargetLanguage(_stttOptions.CurrentValue.TargetLanguage);
        }

        /// <summary>
        /// Convert speech to text and translate it to the target language
        /// </summary>
        /// <param name="audioFile"></param>
        /// <returns>A string of the transcribed and translated audio</returns>
        public async Task<string> ConvertAndTranslateSpeechToText(IFormFile audioFile)
        {
            string translatedText = string.Empty;
            using (var fileStream = new FileStream("audio.wav", FileMode.Create)) // TODO: Use a temp file
            {
                await audioFile.CopyToAsync(fileStream);
            }
            using var audioConfig = AudioConfig.FromWavFileInput("audio.wav");
            using var translationRecognizer = new TranslationRecognizer(speechTranslationConfig, audioConfig);
            var result = await translationRecognizer.RecognizeOnceAsync();
            if (result.Reason == ResultReason.TranslatedSpeech)
            {
                translatedText = result.Translations[_stttOptions.CurrentValue.TargetLanguage];
            }
            _logger.LogInformation($"Translating from {_stttOptions.CurrentValue.SourceLanguage} to {_stttOptions.CurrentValue.TargetLanguage}");
            return translatedText;
        }
    }
}

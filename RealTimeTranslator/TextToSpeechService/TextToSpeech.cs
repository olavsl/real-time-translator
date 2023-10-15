using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;
using RealTimeTranslator.Options;

namespace RealTimeTranslator.TextToSpeechService
{
    public class TextToSpeech
    {
        private readonly ILogger<TextToSpeech> _logger;
        private readonly IOptionsMonitor<TTSOptions> _ttsOptions;

        private readonly SpeechConfig speechConfig;

        public TextToSpeech(ILogger<TextToSpeech> logger, IOptionsMonitor<TTSOptions> ttsOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ttsOptions = ttsOptions ?? throw new ArgumentNullException(nameof(ttsOptions));
            speechConfig = SpeechConfig.FromSubscription(_ttsOptions.CurrentValue.SubscriptionKey, _ttsOptions.CurrentValue.Region);
        }

        /// <summary>
        /// Converts text to speech
        /// </summary>
        /// <param name="text"></param>
        /// <returns>A .wav file</returns>
        /// <exception cref="Exception"></exception>
        public async Task<byte[]> ConvertTextToSpeech(string text)
        {
            // Save the synthesized audio data as a wave file in this folder.
            using AudioConfig audioConfig = AudioConfig.FromWavFileOutput("output.wav");
            using SpeechSynthesizer synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
            SpeechSynthesisResult result = await synthesizer.SpeakTextAsync(text);
            byte[] audio = result.AudioData;

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                return audio;
            }
            else
            {
                throw new Exception($"Speech synthesis failed: {result.Reason}");
            }
        }
    }
}

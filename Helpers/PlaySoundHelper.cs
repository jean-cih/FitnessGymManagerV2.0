using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    public class PlaySoundHelper
    {
        private WaveOutEvent outputDevice;
        private MediaFoundationReader audioFile;
        private string _path = string.Empty;
        private bool _isSuccess;

        private string soundErrorPath = Properties.Settings.Default.ErrorSoundPath;
        private string soundSuccessPath = Properties.Settings.Default.SuccessSoundPath;

        public PlaySoundHelper(bool isSuccess = true)
        {
            _isSuccess = isSuccess;
            _path = _isSuccess ? soundSuccessPath : soundErrorPath;
        }

        public void PlaySound()
        {
            SystemSound typeSound = _isSuccess ? SystemSounds.Exclamation : SystemSounds.Beep;

            if (!string.IsNullOrEmpty(_path) && File.Exists(_path))
            {
                try
                {
                    MediaFoundationApi.Startup();

                    StopSound();

                    outputDevice = new WaveOutEvent();

                    audioFile = new MediaFoundationReader(_path);
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                }
                catch (Exception ex)
                {
                    typeSound.Play();
                    MessageHelper.MessageWindowOk($"Не удалось воспроизвести звук: {ex.Message}", "Ошибка");
                    StopSound();
                }
            }
            else
            {
                typeSound.Play();
            }
        }

        private void StopSound()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
        }
    }
}

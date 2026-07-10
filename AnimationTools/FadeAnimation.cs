using System;
using System.Windows.Forms;

namespace GymApplicationV2._0.AnimationTools
{
    public class FadeAnimation
    {
        private Timer _fadeTimer;
        private Form _form;
        private float _opacity = 0;
        private float _step = 0.05f;
        private int _interval = 10;
        private Action _onComplete;
        private bool _isClosing = false;

        public FadeAnimation(Form form)
        {
            _form = form;
        }

        public FadeAnimation(Form form, float step = 0.05f, int interval = 10)
        {
            _form = form;
            _step = step;
            _interval = interval;
        }

        public void FadeIn(Action onComplete = null)
        {
            if (_fadeTimer != null)
            {
                Stop();
            }

            _onComplete = onComplete;
            _opacity = 0;
            _form.Opacity = 0;

            _fadeTimer = new Timer();
            _fadeTimer.Interval = _interval;
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity += _step;
                _form.Opacity = Math.Min(_opacity, 1);

                if (_opacity >= 1)
                {
                    _fadeTimer.Stop();
                    _fadeTimer.Dispose();
                    _fadeTimer = null;
                    _onComplete?.Invoke();
                }
            };
            _fadeTimer.Start();
        }

        public void FadeOut(Action onComplete = null)
        {
            if (_fadeTimer != null)
            {
                Stop();
            }

            _onComplete = onComplete;
            _opacity = 1;
            _form.Opacity = 1;

            _fadeTimer = new Timer();
            _fadeTimer.Interval = _interval;
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity -= _step;
                _form.Opacity = Math.Max(_opacity, 0);

                if (_opacity <= 0)
                {
                    _fadeTimer.Stop();
                    _fadeTimer.Dispose();
                    _fadeTimer = null;
                    _onComplete?.Invoke();
                }
            };
            _fadeTimer.Start();
        }

        public void CloseWithAnimation(Action onComplete = null)
        {
            if (_isClosing) return;
            _isClosing = true;

            if (_fadeTimer != null)
            {
                Stop();
            }

            _onComplete = onComplete;
            _opacity = 1;
            _form.Opacity = 1;

            _fadeTimer = new Timer();
            _fadeTimer.Interval = _interval;
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity -= _step;
                _form.Opacity = Math.Max(_opacity, 0);

                if (_opacity <= 0)
                {
                    _fadeTimer.Stop();
                    _fadeTimer.Dispose();
                    _fadeTimer = null;

                    _form.Close();
                    _isClosing = false;

                    _onComplete?.Invoke();
                }
            };
            _fadeTimer.Start();
        }

        public void HideWithAnimation(Action onComplete = null)
        {
            if (_fadeTimer != null)
            {
                Stop();
            }

            _onComplete = onComplete;
            _opacity = 1;
            _form.Opacity = 1;

            _fadeTimer = new Timer();
            _fadeTimer.Interval = _interval;
            _fadeTimer.Tick += (s, e) =>
            {
                _opacity -= _step;
                _form.Opacity = Math.Max(_opacity, 0);

                if (_opacity <= 0)
                {
                    _fadeTimer.Stop();
                    _fadeTimer.Dispose();
                    _fadeTimer = null;

                    _form.Hide();
                    _form.Opacity = 1; 

                    _onComplete?.Invoke();
                }
            };
            _fadeTimer.Start();
        }

        public void Stop()
        {
            if (_fadeTimer != null)
            {
                _fadeTimer.Stop();
                _fadeTimer.Dispose();
                _fadeTimer = null;
            }
            _isClosing = false;
        }

        public void Dispose()
        {
            Stop();
            _form = null;
            _onComplete = null;
        }
    }
}
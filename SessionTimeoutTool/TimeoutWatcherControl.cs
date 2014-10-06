using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SessionTimeoutTool
{
    public class TimeoutWatcherControl : WebControl
    {
        public enum mode
        {
            PageRedirect,
            PopupMessage,
            ExtendTime,
            CustomHandler
        }

        private mode _timeoutMode = mode.CustomHandler;

        private string _redirectPage;
        private string _message;
        private string _popupCSSClass = "";

        public event EventHandler Timeout;

        private int _interval = 1000;
        private readonly int MINUTES = 60000;
        private readonly int SECONDS = 1000;

        protected Timer _sessionTimer = null;
        private UpdatePanel _timeoutPanel = null;

        public string RedirectPage
        {
            get { return _redirectPage; }
            set { _redirectPage = value; }
        }

        public string TimeoutMessage
        {
            get { return _message; }
            set { _message = value; }
        }

        public string PopupCSSClass
        {
            get { return _popupCSSClass; }
            set { _popupCSSClass = value; }
        }

        public bool TimerEnabled
        {
            get
            {
                return Convert.ToBoolean(ViewState[this.ID + "TimedOutFlag"] ?? true);
            }
            set
            {
                GetSessionTimer().Enabled = value;
                ViewState[this.ID + "TimedOutFlag"] = value;
            }
        }

        public mode TimeoutMode
        {
            get { return _timeoutMode; }
            set { _timeoutMode = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            var state = HttpContext.Current.Session;

            this._interval = state.Timeout * MINUTES;

            var timeoutPanel = GetTimeoutPanel();
            var sessionTimer = GetSessionTimer();
            sessionTimer.Interval = TimeoutMode == mode.ExtendTime ?
                _interval - (45 * SECONDS) :
                _interval;


            timeoutPanel.ContentTemplateContainer.Controls.Add(sessionTimer);

            this.Controls.Add(timeoutPanel);
        }

        void _sessionTimer_Tick(object sender, EventArgs e)
        {
            switch (TimeoutMode)
            {
                case mode.ExtendTime:
                    // Do nothing
                    break;
                case mode.PageRedirect:
                    DisableTimer();
                    Redirect(this.RedirectPage);
                    break;
                case mode.PopupMessage:
                    DisableTimer();
                    BuildPopup();
                    break;
                case mode.CustomHandler:
                default:
                    DisableTimer();
                    OnTimeout();
                    break;
            }
        }

        private void OnTimeout()
        {
            if (Timeout != null)
            {
                Timeout(this, EventArgs.Empty);
            }
        }

        private void BuildPopup()
        {
            throw new NotImplementedException();
        }

        private void DisableTimer()
        {
            TimerEnabled = false;
        }

        private void Redirect(string redirectPage)
        {
            if (!string.IsNullOrEmpty(redirectPage))
            {
                Context.Response.Redirect(
                    VirtualPathUtility.ToAbsolute(redirectPage));
            }
        }

        private Timer GetSessionTimer()
        {
            if (_sessionTimer == null)
            {
                _sessionTimer = new Timer();
                _sessionTimer.Tick += _sessionTimer_Tick;
                _sessionTimer.Enabled = this.TimerEnabled;
                _sessionTimer.ID = this.ID + "SessionTimeoutTimer";
            }
            return _sessionTimer;
        }

        private UpdatePanel GetTimeoutPanel()
        {
            if (_timeoutPanel == null)
            {
                _timeoutPanel = new UpdatePanel();
                _timeoutPanel.ID = this.ID + "SessionTimeoutPanel";
                _timeoutPanel.UpdateMode = UpdatePanelUpdateMode.Always;
            }
            return _timeoutPanel;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrightness.Model
{
    internal class ApplicationSettingsModel : ModelBase
    {
        private string _applicationName;
        private string _applicationTitle;
        private int _applicationBrightness;
        private int _EnableHDR;

        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; RaisePropertyChanged(nameof(ApplicationName)); }
        }

        public string ApplicationTitle
        {
            get { return _applicationTitle; }
            set { _applicationTitle = value; RaisePropertyChanged(nameof(ApplicationTitle)); }
        }

        public int ApplicationBrightness
        {
            get { return _applicationBrightness; }
            set { _applicationBrightness = value; RaisePropertyChanged(nameof(ApplicationBrightness)); }
        }

        public int EnableHDR
        {
            get { return _EnableHDR; }
            set { _EnableHDR = value; RaisePropertyChanged(nameof(EnableHDR)); }
        }

        public ApplicationSettingsModel(string _applicationName,string _applicationTitle, int _applicationBrightness, int _EnableHDR)
        {
            this.ApplicationName = _applicationName;
            this.ApplicationTitle = _applicationTitle;
            this.ApplicationBrightness = _applicationBrightness;
            this.EnableHDR = _EnableHDR;
        }
    }
}

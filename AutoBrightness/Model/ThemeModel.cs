using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrightness.Model
{
    internal class ThemeModel : ModelBase
    {
        private string _themeName;

        public string ThemeName
        {
            get { return _themeName; }
            set { _themeName = value; RaisePropertyChanged(ThemeName); }
        }
    }
}

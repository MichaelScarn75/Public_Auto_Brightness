using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBrightness.Model;
using Newtonsoft.Json;

namespace AutoBrightness.ViewModel
{
    internal class AutoBrightnessSettingsViewModel : ViewModelBase
    {
        private ObservableCollection<ApplicationSettingsModel> _currentActiveAppsModels_IEnum;
        private ObservableCollection<ApplicationSettingsModel> _addedAppsModels_IEnum;
        private ObservableCollection<ThemeModel> _themeModels_IEnum;

        public ObservableCollection<ApplicationSettingsModel> CurrentActiveAppsModels_IEnum
        {
            get { return _currentActiveAppsModels_IEnum; }
            set { _currentActiveAppsModels_IEnum = value; RaisePropertyChanged(nameof(CurrentActiveAppsModels_IEnum)); OCPropertyChanged(nameof(CurrentActiveAppsModels_IEnum)); }
        }

        public ObservableCollection<ApplicationSettingsModel> AddedAppsModels_IEnum
        {
            get { return _addedAppsModels_IEnum; }
            set { _addedAppsModels_IEnum = value; RaisePropertyChanged(nameof(AddedAppsModels_IEnum)); OCPropertyChanged(nameof(AddedAppsModels_IEnum)); }
        }

        public ObservableCollection<ThemeModel> ThemeModels_IEnum
        {
            get { return _themeModels_IEnum; }
            set { _themeModels_IEnum = value; RaisePropertyChanged(nameof(ThemeModels_IEnum)); OCPropertyChanged(nameof(ThemeModels_IEnum)); }
        }

        public AutoBrightnessSettingsViewModel()
        {
            this.CurrentActiveAppsModels_IEnum = new();
            this.CurrentActiveAppsModels_IEnum = ForegroundAppInfo.ShowAllApplications();
            this.AddedAppsModels_IEnum = new();
            LoadJson();
        }

        public void populate_data()
        {
            this.CurrentActiveAppsModels_IEnum = new();
            this.CurrentActiveAppsModels_IEnum = ForegroundAppInfo.ShowAllApplications();
            this.AddedAppsModels_IEnum = new();
            LoadJson();
        }

        public void LoadJson()
        {
            using (StreamReader r = new StreamReader(Path.Combine(AppContext.BaseDirectory, @"ActiveList.json")))
            {
                string json = r.ReadToEnd();
                List<ApplicationSettingsModel> items = JsonConvert.DeserializeObject<List<ApplicationSettingsModel>>(json);

                if (items != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        this.AddedAppsModels_IEnum.Add(
                            new ApplicationSettingsModel(
                                item.ApplicationName,
                                item.ApplicationTitle,
                                item.ApplicationBrightness,
                                item.EnableHDR
                                )
                            );
                    }
                }
            }
        }
    }
}

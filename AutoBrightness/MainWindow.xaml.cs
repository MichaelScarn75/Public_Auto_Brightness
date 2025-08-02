using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AutoBrightness.ViewModel;
using AutoBrightness.Model;
using System.IO;
using System.Reflection;

namespace AutoBrightness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread newthread;
        bool stopThread = false;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                SfSkinManager.SetTheme(this, new Theme("Windows11Dark"));
                SetStartup();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void ComboBoxAdv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SfSkinManager.SetTheme(this, new Theme(((ComboBoxItemAdv)ComboBox1.SelectedItem).Content.ToString()));
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        public void Start()
        {
            try
            {
                while (true)
                {
                    if (stopThread)
                    {
                        return;
                    }

                    Thread.Sleep(800);

                    this.Dispatcher.Invoke(() =>
                    {
                        var datacontext = (this.DataContext as AutoBrightnessSettingsViewModel);
                        var success = false;

                        if (datacontext.AddedAppsModels_IEnum.Count == 0)
                        {
                            return;
                        }

                        foreach (var item in datacontext.AddedAppsModels_IEnum)
                        {
                            if (ForegroundAppInfo.ReturnActiveApplicationProcessName().Contains(item.ApplicationName) &&
                                MonitorBrightnessController.ShowCurrentBrightness() != (uint)item.ApplicationBrightness
                            )
                            {
                                MonitorBrightnessController.SetBrightness((uint)item.ApplicationBrightness);


                                success = true;
                                break;
                            }
                        }

                        if (!success)
                        {
                            MonitorBrightnessController.SetBrightness(0);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    ShowErrorMessage(ex.Message.ToString());
                });
            }
        }

        private BitmapImage ReturnBitmap(string _filepath)
        {
            return new System.Windows.Media.Imaging.BitmapImage(
            new Uri(string.Format($"{_filepath}"), UriKind.RelativeOrAbsolute));
        }

        private string AppResourceLocator(string _filepath)
        {
            return Path.Combine(AppContext.BaseDirectory, _filepath);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string _filepath0 = AppResourceLocator(@"Images\light-bulb.png");
                string _filepath = AppResourceLocator(@"Images\Arrow_Blue2.png");

                MainWindow_Window.Icon = ReturnBitmap(_filepath0);

                AddButton.LargeIcon = ReturnBitmap(_filepath);

                AddButton.SmallIcon = ReturnBitmap(_filepath);

                var datacontext = (this.DataContext as AutoBrightnessSettingsViewModel);
                datacontext.populate_data();
                this.newthread = new Thread(new ThreadStart(Start));
                this.newthread.Start();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void ButtonAdv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stopThread = true;
                Thread.Sleep(800);  //gives the loop thread enough time to terminate
                var datacontext = (this.DataContext as AutoBrightnessSettingsViewModel);
                foreach (ApplicationSettingsModel item in SfDataGrid1.SelectedItems)
                {
                    if (datacontext.AddedAppsModels_IEnum
                        .Where(a => a.ApplicationName.Equals(item.ApplicationName))
                        .Count() == 0
                        )
                    {
                        datacontext.AddedAppsModels_IEnum.Add(item);
                    }
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(datacontext.AddedAppsModels_IEnum.ToList(), options);
                File.WriteAllTextAsync(Path.Combine(AppContext.BaseDirectory, @"ActiveList.json"), json);
                datacontext.populate_data();
                StartTheThread();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void SfDataGrid2_CurrentCellEndEdit(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellEndEditEventArgs e)
        {
            try
            {
                stopThread = true;
                Thread.Sleep(800);  //gives the loop thread enough time to terminate
                var datacontext = (this.DataContext as AutoBrightnessSettingsViewModel);
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(datacontext.AddedAppsModels_IEnum.ToList(), options);
                File.WriteAllTextAsync(Path.Combine(AppContext.BaseDirectory, @"ActiveList.json"), json);
                datacontext.populate_data();
                StartTheThread();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void SfDataGrid2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Delete ||
                    Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Delete)
                {
                    stopThread = true;
                    Thread.Sleep(800);  //gives the loop thread enough time to terminate
                    var datacontext = (this.DataContext as AutoBrightnessSettingsViewModel);

                    foreach (ApplicationSettingsModel item in SfDataGrid2.SelectedItems.ToList())
                    {
                        datacontext.AddedAppsModels_IEnum.Remove(item);
                    }

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(datacontext.AddedAppsModels_IEnum.ToList(), options);
                    File.WriteAllTextAsync(Path.Combine(AppContext.BaseDirectory, @"ActiveList.json"), json);
                    datacontext.populate_data();
                    StartTheThread();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                stopThread = true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void StartTheThread()
        {
            try
            {
                if (!this.newthread.IsAlive ||
                    this.newthread.ThreadState.ToString() == "WaitSleepJoin"
                    )
                {
                    stopThread = false;
                    this.newthread = new Thread(new ThreadStart(Start));
                    this.newthread.Start();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message.ToString());
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBoxResult result = MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string exelocation = Assembly.GetExecutingAssembly().Location.ToString().Replace("dll", "exe");
            rk.SetValue("Auto Brightness", "\"" + exelocation + "\"" + " -minimized");

            /*
            if (chkStartUp.Checked)
                rk.SetValue(AppName, Application.ExecutablePath);
            else
                rk.DeleteValue(AppName, false);
            */

        }
    }
}
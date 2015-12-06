﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DefaultMWMP2toolbar;
using LoadablePlugin;
using PluginLibrary.Customization;
using SidePluginLoader.Annotations;

namespace SidePluginLoader
{
    public class PluginLoader : INotifyPropertyChanged
    {
        #region Constructor

        public PluginLoader(ILoadablePlugin loadable, Action<PluginLoader> onSelection)
        {
            PluginName = loadable.PresentationName;
            PluginLogo = loadable.Logo as UIElement;
            Plugin = loadable;

            OnSelect = new UiCommand(delegate { onSelection(this); });
        }

        #endregion

        #region Ui Plugin based

        public string PluginName { get; }
        public UIElement PluginLogo { get; }

        #endregion

        #region Ui Control based

        private bool _pluginSelected = false;
        public bool PluginSelected { get { return _pluginSelected; } set { _pluginSelected = value; OnPropertyChanged(nameof(PluginSelected)); } }

        #endregion

        #region Loadable Plugin

        public ILoadablePlugin Plugin;

        #endregion


        private UiCommand _onSelect = null;
        public UiCommand OnSelect { get { return _onSelect; } set { _onSelect = value; OnPropertyChanged(nameof(OnSelect)); } }


        #region Notifier Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
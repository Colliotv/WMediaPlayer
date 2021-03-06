﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Video
{
    [Serializable]
    public class Track : MediaPropertiesLibrary.TrackDefinition
    {

        #region Serializable Members

        public string Name { get; set; }
        public string Path { get; set; }

        [XmlElement("Duration")]
        public long SerializedDuration { get { return Duration.Ticks; } set { Duration = new TimeSpan(value); } }

        [XmlArray("Subtitles")]
        public ObservableCollection<Subtitle> Subtitles { get; } = new ObservableCollection<Subtitle>();

        #endregion

        #region Non Serializable Members

        [XmlIgnore]
        public List<string> RelativePath { get; set; }

        [XmlIgnore]
        public override TimeSpan Duration { get; set; }

        [XmlIgnore]
        public Serie Serie { get; set; }

        [XmlIgnore]
        public BitmapImage Picture { get; set; }

        #endregion


        #region Herited From ITracks

        [XmlIgnore]
        public override string MediaLibraryKey => Path;

        protected override void OnStateChanged(MediaState state)
        {
            if (state == MediaState.End) return;;
            if (Serie != null)
                Serie.State = state;
        }

        #endregion
    }

    [Serializable]
    public class Subtitle
    {
        public string Path { get; set; }
        public string Name { get; set; }
    }


    public class TrackAccessSerieName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Track)?.Serie?.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TrackDurationStylized : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;
            if (track == null)
                return "";
            return track.Duration > TimeSpan.FromHours(1) ? track.Duration.ToString(@"hh\:mm\:ss") : track.Duration.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
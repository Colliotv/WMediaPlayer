﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Audio
{
    public class Track : ITrack
    {
        #region Serializable Members

        public string Name { get; set; }
        public string Path { get; set; }
        public TrackUserTag UserTag { get; set; }

        [XmlIgnore]
        public TimeSpan Duration { get; set; }

        [XmlElement("Duration")]
        public long TimeSpanDuration { get { return Duration.Ticks; } set { Duration = new TimeSpan(value); } }

        #endregion

        #region Attached Members

        [XmlIgnore]
        public Album Album { get; set; }
        [XmlIgnore]
        public List<Artist> Artist { get; set; }
        [XmlIgnore]
        public List<string> RelativePaths { get; set; }

        #endregion

        #region Herited From ITracks

        public string MediaLibraryKey => Path;

        #endregion

    }

    public sealed class TrackUserTag
    {
    }

    public class TrackAccessArtistName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;
            if (track == null)
                return "";
            return track.Artist?.Count > 0 ? track.Artist[0].Name : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TrackAccessAlbumName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Track)?.Album?.Name;
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
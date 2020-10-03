using System;
using System.Linq;
using System.Collections.Generic;
using Athame.Plugin.Api.Service;

namespace Athame.Core.Utilities
{
    public static class MediaSample
    {
        private static readonly RandomWords randomWords = new RandomWords();

        public static Album Album 
            => GetSampleAlbum();

        public static Playlist Playlist
            => GetSamplePlaylist();
        
        private static Album GetSampleAlbum()
        {
            var album = new Album
            {
                Title = randomWords.NewTitle(),
                Artist = new Artist
                {
                    Name = randomWords.NewArtistName()
                },
                Tracks = new List<Track>()
            };

            album.Tracks.Add(new Track
            {
                Title = randomWords.NewTitle(),
                Album = album,
                Artist = album.Artist,
                DiscNumber = 1,
                TrackNumber = 1,
                Genre = "Genre",
                Composer = new string[]
                {
                    randomWords.NewFullName()
                },
                Year = DateTime.Now.Year
            });

            return album;
        }

        private static Playlist GetSamplePlaylist()
        {
            var playlist = new Playlist
            {
                Title = randomWords.NewVerbNounTitle(),
                Tracks = new List<Track>()
            };

            var track = new Track
            {
                Playlist = playlist,
                Title = randomWords.NewTitle(),
                Album = new Album
                {
                    Artist = new Artist 
                    { 
                        Name = randomWords.NewArtistName() 
                    },
                    Title = randomWords.NewTitle(),
                    Tracks = new List<Track>()
                },
                DiscNumber = 1,
                TrackNumber = 1,
                Genre = "Genre",
                Composer = new string[]
                {
                    randomWords.NewFullName()
                },
                Year = DateTime.Now.Year
            };

            track.Artist = track.Album.Artist;
            track.Album.Tracks.Add(track);

            playlist.Tracks.Add(track);
            return playlist;
        }
    }
}
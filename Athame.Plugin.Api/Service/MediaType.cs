﻿namespace Athame.Plugin.Api.Service
{
    public enum MediaType
    {
        Unknown = 0,

        Album = 1 << 0,

        Track = 1 << 1,

        Playlist = 1 << 2,

        Artist = 1 << 3
    }
}

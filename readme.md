[Athame](https://svbnet.co/athame)
======
<a href='https://ko-fi.com/A0343RZ9' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi2.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

Bitcoin: `3BTjhi5pCUJcqxSgKDnJF4ct4LYwmX2Wa7`

Athame is a program for downloading music from music streaming and sharing services.
It is intended for educational and private use only, and **not** as a tool for pirating and distributing music.
Above all else, remember that the artists and studios put a lot of work into making music -- if you can, purchase
music directly from the artist as more money goes to them.

Since I am also caught up with other things I can't devote all my time to fixing and improving Athame. Right now it is
just a very buggy, basic tool which I hope will either be improved upon in the future, or be contributed to.

Download
--------
[Click the 'Releases' tab above at the top to download the latest, or just click here.](https://github.com/svbnet/Athame/releases)

FAQ
---
### Can you add feature x?
I am currently working on a newer, WPF-based release of Athame which has all of the features of the current release plus features
like artist browsing, text searching, and so on.

### Can you add plugin x?
There are hundreds of music streaming services out there, so I'll mainly focus on the two plugins listed below.

### Can you remove the "Respect the artists! Pay for music when you can! Downloaded with Athame" tag?
No. The purpose of that tag is to remind people that Athame is a tool for **saving music from streaming services for personal use only**, and not a tool
for illegal distribution of music.

### Why can't I add an artist?
This is a feature being worked on in the newer version. The issue is that currently there is no way to determine what an artist URL definitely points to - the user
may want the artist's top tracks, or the artist's discography. Even with a UI where the user can decide what they want, there is still the issue where duplicate tracks
from singles are added, or the artist has a lot of albums the user will have to manually delete. In the newer version, artists will be displayed with top tracks and albums.
The user can then manually select which album or tracks they want to download.

### Why is Tidal Master Quality audio not really that different to Hi-Fi (FLAC) quality?
Tidal Master albums are stored in a format called [MQA](https://en.wikipedia.org/wiki/Master_Quality_Authenticated). MQA is basically a fancy FLAC file that has the
higher frequencies encoded in the standard FLAC stream, meaning that it can still be played back on players that don't support it directly. Unfortunately MQA is more of an
"exclusive" format that at the moment requires specialised software and/or to enjoy its full benefits. The MQA files downloaded by Athame are provided more as an experiment for anyone
wanting to dissect them and perhaps one day build a proper decoder.

### Why do some Tidal lossless files look like they've been transcoded from lossy?
### Why does Tidal return a 96kbps MP3/AAC when I have my quality setting on High, etc, etc?
Athame and the Tidal plugin don't do any post-processing on the audio file, apart from setting tags. A few albums don't have a lossless version, and some are in MP3 format for some reason
too. Either way the plugin retrieves audio streams the same way that the web and mobile players retrieve them, so as far as I know this is entirely to do with Tidal. Some FLAC tracks,
when put into a spectrum analyzer, also seem to cut off around 20Hz like a lossy file would.


Plugins
-------
### [Tidal](https://github.com/svbnet/AthamePlugin.Tidal/releases) and [Google Play Music](https://github.com/svbnet/AthamePlugin.GooglePlayMusic/releases)
These are included by default

### Deezer
A Deezer plugin was created at some point, but it's since been taken down by Deezer/GitHub since they seem to have a pretty stringent DMCA policy. I would advise that nobody publicly release an Athame Deezer plugin, as it will most likely
get taken down within a short amount of time.

### Spotify
Currently, I have no plans to work on a Spotify plugin. A similar thing called [librespot](https://github.com/plietar/librespot) exists, so
maybe one day this could be ported to C#.

### Apple Music
As far as I know, nobody has documented the Apple Music API yet, so nope.

### Qobuz
Qobuz isn't available in my country at the moment, so I have no plans to implement it.

**At the moment, I am mostly working on the Athame core application, so I can't spend my time writing plugins for other services.**

Plugins are always distributed as Zip files - to install a plugin, simply extract the zip to the "Plugins" folder, which is in the same directory
as the Athame executable. A guide for creating your own plugins can be found on the wiki.

Then...
-------
Open the Athame.exe executable.

Keeping up to date
------------------
Follow me on [Twitter @svbnet](https://twitter.com/svbnet) and subscribe to [my blog](https://blog.svbnet.co) to stay up to date.

Usage
-----
Enter a URL in the "URL" textbox, then click "Add". It will show up in the download queue. Click "Start" to begin downloading.

If you haven't signed in, you can click the `Menu` button, then go to `Settings` and choose the tab of the music service
you want to sign into. You can also just enter a URL and click "Click here to sign in" on the error message below the URL
textbox.

Under `Settings > General`, you can change where music is downloaded to as well as the filename format used. There is an explanation
of the valid format specifiers on the General tab.

Build
-----
* .NET 4.6.2 or later
* Visual Studio 2015 (Express will work fine) or later with NuGet

Roadmap
-------
While Athame currently uses WinForms for its UI, this is a halfway solution to an ideal UI. I'm currently in the process of creating a
WPF UI to replace the WinForms UI, which will hopefully also fix many bugs in the process. I am also currently considering a cross-platform
interface (since WinForms is incredibly buggy on non-Windows platforms), but again this is just a consideration as writing a command-line
interface would take time away from porting it to WPF. A GTK# interface would also be possible, but would take a while since I am unfamiliar with GTK#.
There are currently no plans for a Cocoa (OS X) interface since I do not have a Mac.
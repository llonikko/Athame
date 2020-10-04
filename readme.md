Athame
------
Athame is a program for downloading music from music streaming and sharing services.
It is intended for educational and private use only, and **not** as a tool for pirating and distributing music.
Above all else, remember that the artists and studios put a lot of work into making music -- if you can, purchase
music directly from the artist as more money goes to them.

Since I am also caught up with other things I can't devote all my time to fixing and improving Athame. Right now it is
just a very buggy, basic tool which I hope will either be improved upon in the future, or be contributed to.

Build
-----
* .NET Core 3.1
* Visual Studio 2019 or VS Code

Roadmap
-------
While Athame currently uses WinForms for its UI, this is a halfway solution to an ideal UI. I'm currently in the process of creating a
WPF UI to replace the WinForms UI, which will hopefully also fix many bugs in the process. I am also currently considering a cross-platform
interface (since WinForms is incredibly buggy on non-Windows platforms), but again this is just a consideration as writing a command-line
interface would take time away from porting it to WPF. A GTK# interface would also be possible, but would take a while since I am unfamiliar with GTK#.
There are currently no plans for a Cocoa (OS X) interface since I do not have a Mac.

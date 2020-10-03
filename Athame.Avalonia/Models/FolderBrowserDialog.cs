using Avalonia.Controls;
using System.Threading.Tasks;

namespace Athame.Avalonia.Models
{
    public class FolderBrowserDialog
    {
        private readonly Window parent;

        public FolderBrowserDialog(Window parent)
        {
            this.parent = parent;
        }

        public async Task<string> SelectFolder(string location)
        {
            if (await SelectFolder() is string folder && !string.IsNullOrEmpty(folder))
            {
                return folder;
            }
            return location;
        }

        public Task<string> SelectFolder()
            => new OpenFolderDialog().ShowAsync(parent);
    }
}
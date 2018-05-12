using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.Core.Platform.Win32;

namespace Athame.Core.Platform
{
    [Flags]
    public enum KeepAwakeRequirement
    {
        Clear = 1 << 0,
        Screen = 1 << 1,
        System = 1 << 2
    }

    public abstract class KeepAwakeContext : IDisposable
    {
        private static readonly Lazy<KeepAwakeContext> Instance;

        static KeepAwakeContext()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Instance = new Lazy<KeepAwakeContext>(() => new Win32KeepAwakeContext());
            }
            else
            {
                throw new PlatformNotSupportedException("This feature is only implemented on Windows.");
            }
        }

        public static KeepAwakeContext Create(KeepAwakeRequirement requirement)
        {
            Instance.Value.SetKeepAwakeRequirement(requirement);
            return Instance.Value;
        }

        public abstract KeepAwakeRequirement GetKeepAwakeRequirement();

        public virtual void SetKeepAwakeRequirement(KeepAwakeRequirement requirement)
        {
            if (requirement == GetKeepAwakeRequirement()) return;
            if ((requirement & KeepAwakeRequirement.Clear) != 0 && (
                    (requirement & KeepAwakeRequirement.Screen) != 0 || (requirement & KeepAwakeRequirement.System) != 0))
            {
                throw new InvalidOperationException("Invalid combination of flags");
            }
        }

        public virtual void Dispose()
        {
            SetKeepAwakeRequirement(KeepAwakeRequirement.Clear);
        }
    }
}

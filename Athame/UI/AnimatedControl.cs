using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Athame.UI
{
    public class AnimatedControl<TControl, TState>
    {
        public TControl Control { get; set; }

        public bool IsAnimating { get; set; }

        public TState PreviousState { get; set; }

        public AnimatedControl(TControl control, TState previousState)
        {
            Control = control;
            IsAnimating = true;
            PreviousState = previousState;
        }
    }
}

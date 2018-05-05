using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Athame.UI
{
    public class ListViewItemAnimator
    {
        private readonly List<AnimatedControl<ListViewItem, int>> items = new List<AnimatedControl<ListViewItem, int>>();
        private readonly Timer timer = new Timer();
        private int startIndex;
        private int endIndex;
        private int currentIndex;
        private int frameRate;
        

        public int FrameRate
        {
            get { return frameRate; }
            set
            {
                frameRate = value;
                timer.Interval = FrameRateMs();
            }
        }

        private void Init(int _startIndex, int _endIndex, int frameRate)
        {
            startIndex = _startIndex;
            endIndex = _endIndex;
            currentIndex = _startIndex;
            FrameRate = frameRate;
            timer.Tick += TimerOnTick;
        }

        public ListViewItemAnimator(int startIndex, int endIndex)
        {
            Init(startIndex, endIndex, 12);
        }

        public ListViewItemAnimator(int startIndex, int endIndex, int frameRate)
        {
            Init(startIndex, endIndex, frameRate);
        }

        public void Add(ListViewItem item)
        {
            var animatedControl = items.FirstOrDefault(control => control.Control == item);
            if (animatedControl != null) return;
            items.Add(new AnimatedControl<ListViewItem, int>(item, item.ImageIndex));
        }

        public bool Remove(ListViewItem item)
        {
            var animatedControl = items.FirstOrDefault(control => control.Control == item);
            if (animatedControl == null) return false;
            animatedControl.IsAnimating = false;
            animatedControl.Control.ImageIndex = animatedControl.PreviousState;
            return true;
        }

        public void Start()
        {
            if (timer.Enabled) return;
            timer.Start();
        }

        public void Stop()
        {
            if (!timer.Enabled) return;
            timer.Stop();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            currentIndex = ++currentIndex > endIndex ? startIndex : currentIndex;
            foreach (var item in items)
            {
                if (item.IsAnimating)
                {
                    item.Control.ImageIndex = currentIndex;
                }
            }
        }

        private int FrameRateMs()
        {
            return (int) (((double) 1 / frameRate) * 1000);
        }
    }
}

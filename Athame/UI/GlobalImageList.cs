using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Athame.UI
{
    [ToolboxItem(false)]
    public class GlobalImageList : Component
    {
        private IContainer components;
        private ImageList imageList;
        private static GlobalImageList _instance;

        public ImageList ImageList => imageList;

        public static GlobalImageList Instance => _instance ?? (_instance = new GlobalImageList());

        private GlobalImageList()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalImageList));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "not_downloadable");
            this.imageList.Images.SetKeyName(1, "ready");
            this.imageList.Images.SetKeyName(2, "done");
            this.imageList.Images.SetKeyName(3, "warning");
            this.imageList.Images.SetKeyName(4, "loading1.png");
            this.imageList.Images.SetKeyName(5, "loading2.png");
            this.imageList.Images.SetKeyName(6, "loading3.png");
            this.imageList.Images.SetKeyName(7, "loading4.png");
            this.imageList.Images.SetKeyName(8, "loading5.png");
            this.imageList.Images.SetKeyName(9, "loading6.png");
            this.imageList.Images.SetKeyName(10, "loading7.png");
            this.imageList.Images.SetKeyName(11, "loading8.png");
            this.imageList.Images.SetKeyName(12, "loading9.png");
            this.imageList.Images.SetKeyName(13, "loading10.png");
            this.imageList.Images.SetKeyName(14, "loading11.png");
            this.imageList.Images.SetKeyName(15, "loading12.png");
            this.imageList.Images.SetKeyName(16, "error");

        }
    }
}

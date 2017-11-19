using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for piceditWindow.xaml
    /// </summary>
    public partial class piceditWindow : Window
    {

        string filePath=null;
        private Point previousMousePoint;
        private bool isMouseLeftButtonDown;
        private Cursor oldCursor;
        private IMAdorner _IMAdorner;
        public piceditWindow(string source)
        {
            InitializeComponent();
            filePath = source;
            getImg();
            //head_portrait.Source = new BitmapImage(new Uri(picSource, UriKind.RelativeOrAbsolute));
        }
        



        #region 成员方法

        public void SetImage(string filePath)
        {
            // Read byte[] from png file
            BinaryReader binReader = new BinaryReader(File.Open(filePath, FileMode.Open));
            FileInfo fileInfo = new FileInfo(filePath);
            byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
            binReader.Close();

            // Init bitmap
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();

            this.MasterImage.Source = bitmap;
        }

        #endregion 成员方法



        public void MasterImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            ContentControl image = sender as ContentControl;
            if (image == null)
            {
                return;
            }

            TransformGroup group = ImageComparePanel.FindResource("ImageCompareResources") as TransformGroup;
            //Debug.Assert(group != null, "Can't find transform group from image compare panel resource");
            Point point = e.GetPosition(image);
            double scale = e.Delta * 0.001;
            ZoomImage(group, point, scale);
        }

        private static void ZoomImage(TransformGroup group, Point point, double scale)
        {
           // Debug.Assert(group != null, "Oops, ImageCompareResources is removed from current control's resouce");

            Point pointToContent = group.Inverse.Transform(point);
            ScaleTransform transform = group.Children[0] as ScaleTransform;

            if (transform.ScaleX + scale < 0.5)
            {
                return;
            }

            transform.ScaleX += scale;
            transform.ScaleY += scale;
            TranslateTransform transform1 = group.Children[1] as TranslateTransform;
            transform1.X = -1 * ((pointToContent.X * transform.ScaleX) - point.X);
            transform1.Y = -1 * ((pointToContent.Y * transform.ScaleY) - point.Y);

        }

        public void MasterImage_MouseMove(object sender, MouseEventArgs e)
        {
            ContentControl image = sender as ContentControl;

            if (image == null)
            {
                return;
            }

            if (this.isMouseLeftButtonDown && e.LeftButton == MouseButtonState.Pressed)
            {
                this.DoImageMove(image, e.GetPosition(image));
            }
        }

        private void DoImageMove(ContentControl image, Point position)
        {

            TransformGroup group = ImageComparePanel.FindResource("ImageCompareResources") as TransformGroup;

           // Debug.Assert(group != null, "Can't find transform group from image compare panel resource");

            TranslateTransform transform = group.Children[1] as TranslateTransform;

            transform.X += position.X - this.previousMousePoint.X;

            transform.Y += position.Y - this.previousMousePoint.Y;

            this.previousMousePoint = position;
        }

        public void MasterImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null) return;

            this.oldCursor = this.Cursor;
            this.Cursor = Cursors.SizeAll;

            img.CaptureMouse();
            this.isMouseLeftButtonDown = true;
            this.previousMousePoint = e.GetPosition(img);
        }

        public void MasterImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null) return;

            this.Cursor = this.oldCursor;

            img.ReleaseMouseCapture();
            this.isMouseLeftButtonDown = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(ImageComparePanel);
            _IMAdorner = new IMAdorner(ImageComparePanel, this);
            layer.Add(_IMAdorner);
        }

       
        private void getImg()
        {
            if (MasterImage.Source == null)
            {

                    BinaryReader binReader = new BinaryReader(File.Open(filePath, FileMode.Open));
                    FileInfo fileInfo = new FileInfo(filePath);
                    byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                    binReader.Close();

                    // Init bitmap
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(bytes);
                    bitmap.EndInit();
                    MasterImage.Source = bitmap;

            }
        }

        //点击保存，裁剪图片
        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            #region 裁剪图片

            //目标效果,即界面选取框相对于界面的坐标以及宽高，界面像素600*600
            // double x1 = 125;
            double x1 = 0;
            //double y1 = 125;
            double y1 = 90;
            //double width1 = 350;
            double width1 = 300;
            //double height1 = 350;
            double height1 = 400;

            //界面呈现效果
            TransformGroup group = ImageComparePanel.FindResource("ImageCompareResources") as TransformGroup;
            TranslateTransform transform1 = group.Children[1] as TranslateTransform;
            ScaleTransform transform = group.Children[0] as ScaleTransform;
            double x2 = transform1.X+20;
            double y2 = transform1.Y+120;//75是图片偏移量，通过qq截图工具比划得出
            double scaleX = transform.ScaleX;
            double scaleY = transform.ScaleY;

            //Image控件被设置成Uniform时会自动将原图缩放，此时需要求出原图与控件呈现图的缩放比例offset
            double width3 = MasterImage.ActualWidth;
            double imagewidth = MasterImage.Source.Width;
            double offset = imagewidth / width3;

            //计算原图的裁剪位置以及宽高
            double x = (x1 - x2 / scaleX) * offset;
            double y = (y1 - y2 / scaleY) * offset;
            double width = (width1 / scaleX) * offset;
            double height = (height1 / scaleY) * offset;

            //剪裁图片
            try
            {
                var ob = System.Drawing.Image.FromFile(filePath);
                var b = new System.Drawing.Bitmap((int)width1, (int)height1);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);
                System.Drawing.Rectangle resultRectangle = new System.Drawing.Rectangle(0, 0, (int)width1, (int)height1);
                System.Drawing.Rectangle sourceRectangle = new System.Drawing.Rectangle((int)(x), (int)(y), (int)(width), (int)(height));
                g.DrawImage(ob, resultRectangle, sourceRectangle, System.Drawing.GraphicsUnit.Pixel);
                string str2 = Environment.CurrentDirectory;
                // b.Save(@"D:\imagecutor.jpg");
                // string savePath = "pack://application:,,,/picture/" + GlobalVariable.userNumber.ToString() + ".jpg";
                string savePath = str2 +"\\"+ UserInfo.employee_id + ".jpg";

                if (File.Exists(savePath))
                {
                    string savePath2 = str2 + "\\" + UserInfo.employee_id + "_temp" + ".jpg";
                    b.Save(@savePath2);
                    //File.Delete(savePath);
                    //File.Replace(GlobalVariable.userNumber.ToString() + "_temp" + ".jpg",
                    //   GlobalVariable.userNumber.ToString() + ".jpg",
                    //   "backup");
                }
                else
                {
                    b.Save(@savePath);
                }
                
               // userPage.refreshpic(savePath);
                GlobalVariable.editPictureDone = "1";
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("头像保存出错"+ex.ToString());
            }

            #endregion 裁剪图片
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariable.editPictureDone = "2";
            this.Close();
        }
    }


    //蒙版修饰器
    class IMAdorner : Adorner
    {
        piceditWindow _parent;
        public double x1;
        public double y1;
        public double width;
        public double height;

        public IMAdorner(UIElement adorned, piceditWindow parent)
            : base(adorned)
        {
            _parent = parent;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            Grid ic = this.AdornedElement as Grid;
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(197, 173, 173, 173));

            x1 = ic.Width;
            y1 = ic.Height;
            width = height = 0;//(ic.Width - 200) / 2;//ic.width-100/2
            dc.DrawRectangle(new SolidColorBrush(), new Pen(scb, width), new Rect(0, 0, x1, y1));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _parent.MasterImage_MouseLeftButtonDown(_parent.TestContentControl1, e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _parent.MasterImage_MouseLeftButtonUp(_parent.TestContentControl1, e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _parent.MasterImage_MouseMove(_parent.TestContentControl1, e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _parent.MasterImage_MouseWheel(_parent.TestContentControl1, e);
        }
    }
}
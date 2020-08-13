using System;
using System.Drawing;
using System.Windows.Forms;

namespace JM  //사용하는 프로젝트의 namespace로 수정
{
    //PictureBox 재정의
    class TransparentControl : PictureBox
    {
        public TransparentControl()
        {
        }

        //1배경 + 2개 이상 컨트롤이 배경을 대상으로 TransParent 사용하면서 겹칠 때
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;

            if (this.Parent != null)
            {
                var index = Parent.Controls.GetChildIndex(this);
                for (var i = Parent.Controls.Count - 1; i > index; i--)
                {
                    var c = Parent.Controls[i];
                    if (c.Bounds.IntersectsWith(Bounds) && c.Visible)
                    {
                        using (var bmp = new Bitmap(c.Width, c.Height, g))
                        {
                            c.DrawToBitmap(bmp, c.ClientRectangle);
                            g.TranslateTransform(c.Left - Left, c.Top - Top);
                            g.DrawImageUnscaled(bmp, Point.Empty);
                            g.TranslateTransform(Left - c.Left, Top - c.Top);
                        }
                    }
                }
            }
        }
    }
}

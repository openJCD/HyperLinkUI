using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HyperLinkUI.Engine.GUI
{
    public class NineSlice
    {
        public Texture2D BaseTexture { get; private set; }
        public Rectangle BindRect;
        Point slice_size;
        int tex_W;
        int tex_H;
        Slice TL;
        Slice TM;
        Slice TR;
        Slice ML;
        Slice C;
        Slice MR;
        Slice BL;
        Slice BM;
        Slice BR;
        public NSDrawMode DrawMode = NSDrawMode.Snug;
        public NineSlice(Texture2D tex, Rectangle bind, int frames)
        {
            BaseTexture = tex;
            tex_W = tex.Width / 3;
            tex_H = tex.Height / 3;
            BindRect = bind;
            slice_size = new Point(tex_W/frames, tex_H);
            TL = new Slice(SliceType.TopLeft, tex, frames);
            TM = new Slice(SliceType.TopMid, tex, frames);
            TR = new Slice(SliceType.TopRight, tex, frames);
            ML = new Slice(SliceType.MiddleLeft, tex, frames);
            C  = new Slice(SliceType.Center, tex, frames);
            MR = new Slice(SliceType.MiddleRight, tex, frames);
            BL = new Slice(SliceType.BottomLeft, tex, frames);
            BM = new Slice(SliceType.BottomMid, tex, frames);
            BR = new Slice(SliceType.BottomRight, tex, frames);
        }
        public void SetFrame(int frame)
        {
            TL.SetFrame(frame);
            TM.SetFrame(frame);
            TR.SetFrame(frame);
            ML.SetFrame(frame);
            C.SetFrame(frame);
            MR.SetFrame(frame);
            BL.SetFrame(frame);
            BM.SetFrame(frame);
            BR.SetFrame(frame);
        }
        public void Draw(SpriteBatch sb)
        {
            //each of these coords doesn't actually represent the corners, 
            //they represent the topleft of each texture's position at the corners.

            Point tl;
           
            Point tr; 

            int bl_x, bl_y;
            Point bl;

            Point br;
            int mid_w, mid_h;
            Point mid_size;

            if (DrawMode == NSDrawMode.Snug)
            {
                tl = BindRect.Location;
                tr = new Point(BindRect.Right - slice_size.X, BindRect.Top);

                bl = new Point(BindRect.Left, BindRect.Bottom - tex_H);
                mid_w = BindRect.Width - slice_size.X * 2;
                mid_h = BindRect.Height - tex_H * 2;
                br = new Point(tr.X, BindRect.Bottom - tex_H);
            } else
            {
                tl = BindRect.Location - slice_size;
                tr = new Point(BindRect.Right, BindRect.Top - tex_H);
                bl = new Point(BindRect.Left - slice_size.X, BindRect.Bottom);
                br = new Point(BindRect.Right, BindRect.Bottom);
                mid_w = BindRect.Width;
                mid_h = BindRect.Height;
            }
            mid_size = new Point(mid_w, mid_h);

            Point br_pnt = new Point(tr.X, BindRect.Bottom - tex_H);
            TL.Draw(sb, new Rectangle(tl, slice_size));
            TM.Draw(sb, new Rectangle(new Point(tl.X + slice_size.X, tl.Y), new Point(mid_w, tex_H)));
            TR.Draw(sb, new Rectangle(new Point(tr.X, tr.Y),slice_size));
            ML.Draw(sb, new Rectangle(new Point(tl.X, tl.Y + slice_size.X), new Point(slice_size.X, mid_h)));
            C.Draw(sb, new Rectangle(new Point(tl.X+slice_size.X, tl.Y + tex_H), BindRect.Size - slice_size));
            MR.Draw(sb, new Rectangle(new Point(tr.X, tr.Y + tex_H), new Point(slice_size.X, mid_h)));
            BL.Draw(sb, new Rectangle(bl, slice_size));
            BM.Draw(sb, new Rectangle(new Point(bl.X + slice_size.X, bl.Y), new Point(mid_w, tex_H)));
            BR.Draw(sb, new Rectangle(br, slice_size));
        }
    }

    public class Slice
    {
        int _frames;
        public Point Pos { get=>SliceTexture.Bounds.Location; }
        public Rectangle SourceRect { get; private set; }
        //public Rectangle Bounds { get => SliceTexture.Bounds; }
        public Texture2D SliceTexture { get; private set; }
        public SliceType SliceType { get; private set; }
        public Slice( SliceType st, Texture2D slice_tx, int frames)
        {
            int slice_tx_w = (slice_tx.Width/3)/frames;
            int slice_tx_h = slice_tx.Height/3; 
            int count = slice_tx_w * slice_tx_h;
            SliceType = st;
            SliceTexture = slice_tx;
            _frames = frames;
            switch (st)
            {
                case SliceType.TopLeft:
                    SourceRect = new Rectangle(0, 0, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.TopMid:
                    SourceRect = new Rectangle(slice_tx_w, 0, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.TopRight:
                    SourceRect = new Rectangle(slice_tx_w * 2, 0, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.MiddleLeft:
                    SourceRect = new Rectangle(0, slice_tx_h, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.Center:
                    SourceRect = new Rectangle(slice_tx_w, slice_tx_h, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.MiddleRight:
                    SourceRect = new Rectangle(slice_tx_w * 2, slice_tx_h, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.BottomLeft:
                    SourceRect = new Rectangle(0, slice_tx_h*2, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.BottomMid:
                    SourceRect = new Rectangle(slice_tx_w, slice_tx_h * 2, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.BottomRight:
                    SourceRect = new Rectangle(slice_tx_w * 2, slice_tx_h * 2, slice_tx_w, slice_tx_h);
                    return;
            }
        }
        void RecalcSource(SliceType st, int offset)
        {
            int slice_tx_w = ((SliceTexture.Width / _frames)/3);
            int slice_tx_h = SliceTexture.Height / 3;
            int bottimright = slice_tx_w * slice_tx_h;

            switch (st)
            {
                case SliceType.TopLeft:
                    SourceRect = new Rectangle(offset, 0, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.TopMid:
                    SourceRect = new Rectangle(slice_tx_w + offset, 0, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.TopRight:
                    SourceRect = new Rectangle(offset + slice_tx_w * 2, 0, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.MiddleLeft:
                    SourceRect = new Rectangle(offset, slice_tx_h, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.Center:
                    SourceRect = new Rectangle(slice_tx_w + offset, slice_tx_h, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.MiddleRight:
                    SourceRect = new Rectangle(offset + slice_tx_w * 2, slice_tx_h, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.BottomLeft:
                    SourceRect = new Rectangle(offset, slice_tx_h * 2, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.BottomMid:
                    SourceRect = new Rectangle(slice_tx_w + offset, slice_tx_h * 2, slice_tx_w, slice_tx_h);
                    return;
                case SliceType.BottomRight:
                    SourceRect = new Rectangle(offset + slice_tx_w * 2, slice_tx_h * 2, slice_tx_w, slice_tx_h);
                    return;
            }
        }
        public void SetFrame(int index)
        {
            RecalcSource(SliceType, index * SliceTexture.Width/_frames);
        }
        public void Draw(SpriteBatch sb, Rectangle destinationRect)
        {
            sb.Draw(SliceTexture, destinationRect, SourceRect, Color.White);
        }
    }
    public enum SliceType
    { 
        // each number value here is a coordinate in the NS grid (e.g. 00 is x0 y0)
        TopLeft=00,
        TopMid=10,
        TopRight=20,
        MiddleLeft=01,
        Center=11, 
        MiddleRight=21,
        BottomLeft=02,
        BottomMid=12,
        BottomRight=22
    }
    public enum NSDrawMode
    {
        Snug, 
        Padded
    }
}

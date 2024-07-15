using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography.X509Certificates;

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

        public NineSlice(Texture2D tex, Rectangle bind)
        {
            BaseTexture = tex;
            tex_W = tex.Width / 3;
            tex_H = tex.Height / 3;
            BindRect = bind;
            slice_size = new Point(tex_W, tex_H);
            TL = new Slice( SliceType.TopLeft, tex);
            TM = new Slice( SliceType.TopMid, tex);
            TR = new Slice( SliceType.TopRight, tex);
            ML = new Slice(SliceType.MiddleLeft, tex);
            C  = new Slice(SliceType.Center, tex);
            MR = new Slice( SliceType.MiddleRight, tex);
            BL = new Slice( SliceType.BottomLeft, tex);
            BM = new Slice(SliceType.BottomMid, tex);
            BR = new Slice(SliceType.BottomRight, tex);
        }

        public void Draw(SpriteBatch sb)
        {
            //each of these coords doesn't actually represent the corners, 
            //they represent the topleft of each texture's position at the corners.
            int tr_x = BindRect.Right - tex_W;
            int tr_y = BindRect.Top;

            int bl_x = BindRect.Left;
            int bl_y = BindRect.Bottom - tex_H;

            int mid_w = BindRect.Width - tex_W * 2;
            int mid_h = BindRect.Height - tex_H * 2;

            Point br_pnt = new Point(tr_x, BindRect.Bottom - tex_H);
            TL.Draw(sb, new Rectangle(BindRect.Location, slice_size));
            TM.Draw(sb, new Rectangle(new Point(BindRect.X + tex_W, BindRect.Y), new Point(mid_w, tex_H)));
            TR.Draw(sb, new Rectangle(new Point(tr_x, tr_y),slice_size));
            ML.Draw(sb, new Rectangle(new Point(BindRect.X, BindRect.Y + tex_W), new Point(tex_W, mid_h)));
            C.Draw(sb, new Rectangle(new Point(BindRect.X+tex_W, BindRect.Y + tex_H), BindRect.Size - slice_size));
            MR.Draw(sb, new Rectangle(new Point(tr_x, tr_y + tex_H), new Point(tex_W, mid_h)));
            BL.Draw(sb, new Rectangle(new Point(bl_x, bl_y), slice_size));
            BM.Draw(sb, new Rectangle(new Point(bl_x + tex_W, bl_y), new Point(mid_w, tex_H)));
            BR.Draw(sb, new Rectangle(br_pnt, slice_size));

        }
    }

    public struct Slice
    {
        public Point Pos { get=>SliceTexture.Bounds.Location; }
        public Rectangle SourceRect { get; private set; }
        //public Rectangle Bounds { get => SliceTexture.Bounds; }
        public Texture2D SliceTexture { get; private set; }
        public SliceType SliceType { get; private set; }
        public Slice( SliceType st, Texture2D slice_tx)
        {
            int slice_tx_w = slice_tx.Width/3;
            int slice_tx_h = slice_tx.Height/3; 
            int count = slice_tx_w * slice_tx_h;
            SliceTexture = slice_tx;

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
}

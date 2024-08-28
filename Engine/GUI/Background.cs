using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace HyperLinkUI.Engine.GUI
{
    public class Background
    {
        public Dictionary<string, BackgroundLayer> Layers;
        /// <summary>
        /// Create a background with one layer that uses the given texture in tiled render mode.
        /// </summary>
        /// <param name="sprite">Tile sprite</param>
        
        public Background()
        {
            Layers = new Dictionary<string, BackgroundLayer>();
        }
        public Background(Texture2D sprite)
        {
            Layers = new Dictionary<string, BackgroundLayer>();
            Layers.Add("base", new BackgroundLayer(sprite, BackgroundRenderMode.Tiled, BackgroundAnimateMode.ScrollSE));
        }

        public void Draw(SpriteBatch sb, Rectangle screenBounds)
        {
            foreach (BackgroundLayer layer in Layers.Values)
            {
                layer.Draw(sb, screenBounds);
            }
        }
        public void Animate(GameTime gt)
        {
            foreach (BackgroundLayer layer in Layers.Values)
            {
                layer.Animate(gt);
                //layer.SetOffset(layer.Offset + new Vector2(AnimateSpeed));
                //Debug.WriteLine(layer.Offset);
            }
        }
        public void AddLayer(string keyname, Texture2D sprite, BackgroundRenderMode renderMode, BackgroundAnimateMode animateMode = BackgroundAnimateMode.None, int spd = 30)
        {
            Layers.Add(keyname, new BackgroundLayer(sprite, renderMode, animateMode));
            Layers[keyname].AnimateSpeed = spd;
        }
        public void SetLayerBlend(string layer, Color c)
        {
            Layers[layer].SetBlendColor(c);
        }
        public void SetLayerOffset(string layer, Vector2 offset)
        {
            Layers[layer].SetOffset(offset);
        }
        public void LayerToBottom(string layer)
        {
            var l = Layers[layer];
            Layers.Remove(layer);
            Layers.Add(layer, l);
        }
        public void Export(string filepath)
        {
            Stream s = new FileStream(filepath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(s);
            JsonSerializer ser = new JsonSerializer();
            ser.Formatting = Formatting.Indented;
            ser.Serialize(sw, this);
            sw.Flush();
            sw.Close();
        }
        public static void Import(string filepath, ContentManager c, out Background bg)
        {
            Stream s = new FileStream(filepath, FileMode.Open);
            TextReader sr = new StreamReader(s);
            bg = JsonConvert.DeserializeObject<Background>(sr.ReadToEnd());
            foreach (BackgroundLayer layer in bg.Layers.Values)
            {
                layer.JSONOnly_LoadTexture(c);
            }
        }
    }
    public enum BackgroundRenderMode
    {
        Tiled, 
        Stretched, 
        Letterboxed
    }
    public enum BackgroundAnimateMode
    {
        None,     //0
        ScrollW,  //1
        ScrollE,  //2
        ScrollN,  //3
        ScrollS,  //4
        ScrollNW, //5
        ScrollNE, //6
        ScrollSE, //7
        ScrollSW  //8
    }
    public class BackgroundLayer 
    {
        public BackgroundRenderMode RenderMode { get; set; }
        public BackgroundAnimateMode AnimateMode { get; set; }
        string tx_path;

        [JsonIgnore]
        public Texture2D Sprite { get; private set; }

        public string TexturePath { get => tx_path; set => tx_path = value; }

        public Vector2 Offset { get; set; }
        public Color BlendColor = Color.White;

        public int AnimateSpeed { get; set; } = 50;
        public BackgroundLayer() { }
        public BackgroundLayer (Texture2D sprite, BackgroundRenderMode renderMode, BackgroundAnimateMode animateMode = BackgroundAnimateMode.None)
        {
            RenderMode = renderMode;
            Sprite = sprite;
            Offset = new Vector2();
            AnimateMode = animateMode;
        }
        public void Draw(SpriteBatch sb, Rectangle screenBounds)
        {
            int tileWidth = Sprite.Width;
            int tileHeight = Sprite.Height;
            int tileCountHori = (screenBounds.Width / Sprite.Width);
            int tileCountVert = screenBounds.Height / Sprite.Height;
            int screenW = screenBounds.Width;
            int screenH = screenBounds.Height;
            switch (RenderMode)
            {
                case BackgroundRenderMode.Tiled:
                    for (int x = 0-tileWidth; x < screenW + tileWidth; x += tileWidth)
                    {
                        for (int y = 0-tileHeight; y < screenH + tileHeight; y += tileHeight)
                        {
                            Rectangle tilestep = new Rectangle(new Point(x, y)+Offset.ToPoint(), Sprite.Bounds.Size);
                            sb.Draw(Sprite, tilestep, BlendColor);
                        }
                    }
                    return;
                case BackgroundRenderMode.Letterboxed:
                    Rectangle render_rect = new Rectangle((screenW - tileWidth) / 2, (screenH - tileHeight) / 2, tileWidth, tileHeight);
                    sb.Draw(Sprite, render_rect, BlendColor);
                    return;
                case BackgroundRenderMode.Stretched:
                    sb.Draw(Sprite, new Rectangle(Offset.ToPoint(), screenBounds.Size), BlendColor);
                    return;
                default:
                    sb.Draw(Sprite, new Rectangle(Offset.ToPoint(), Sprite.Bounds.Size), BlendColor);
                    return;
            }
        }
        public void Animate(GameTime gt)
        {
            float x = Offset.X;
            float y = Offset.Y;
            int w = Sprite.Width;
            int h = Sprite.Height;

            switch (AnimateMode)
            {
                case (BackgroundAnimateMode.ScrollN):
                    y -= AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (y < 0 - h)
                        y = h;
                    Offset = new Vector2(x, y);
                    return;
                case (BackgroundAnimateMode.ScrollS):
                    y += AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (y > h)
                        y = -h;
                    Offset = new Vector2(x, y);
                    return;
                case (BackgroundAnimateMode.ScrollW):
                    x -= AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (x < 0 - w)
                        x = w;
                    Offset = new Vector2(x, y);
                    return;
                case (BackgroundAnimateMode.ScrollE):
                    x += AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (x > w)
                        x = -w;
                    Offset = new Vector2(x, y);
                    //Debug.WriteLine(Offset);
                    return;
                case (BackgroundAnimateMode.ScrollNW):
                    x -= AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    y -= AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (x < -w && y < -h)
                    { 
                        x = w; y = h;
                    }
                    Offset = new Vector2(x, y);
                    return;
                case (BackgroundAnimateMode.ScrollNE):
                    x += AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    y -= AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (x > w && y < -h)
                    {
                        x = -w;
                        y = h;
                    }
                    Offset = new Vector2(x, y);
                    return;
                case (BackgroundAnimateMode.ScrollSE):
                    x += AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    y += AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (x > w && y > h)
                    {
                        x = -w; y = -h;
                    }
                    Offset = new Vector2(x, y);
                    return;
                case (BackgroundAnimateMode.ScrollSW):
                    x -= AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    y += AnimateSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                    if (x < -w && y > h)
                    {
                        x = w; y = -h;
                    }
                    Offset = new Vector2(x, y);
                    return;
                default:
                    x = 0;
                    y = 0;
                    Offset = new Vector2(x, y);
                    return;
            }
        }
        public void SetBlendColor(Color c)
        {
            BlendColor = c;
        }
        public void SetOffset(Vector2 o)
        {
            Offset = o;
        }
        internal void JSONOnly_LoadTexture(ContentManager c)
        {
            Sprite = c.Load<Texture2D>(tx_path);
        }
    }
}

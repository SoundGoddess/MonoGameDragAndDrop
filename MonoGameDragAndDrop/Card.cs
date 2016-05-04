using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameDragAndDrop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;

namespace MonoGameDragAndDrop {

    public enum ZOrder {
        Background,
        Normal,
        Child,
        InFront
    }

    class Card : IDragAndDropItem {

        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Card Child { get; }
        public bool IsDraggable { get; set; } = true;
        public ZOrder ZIndex { get; set; } = ZOrder.Normal;
        public int stackValue;


        private readonly SpriteBatch spriteBatch;

        public bool IsSelected { get; set; }
        public bool IsMouseOver { get; set; }
        public bool Contains(Vector2 pointToCheck) {
            Point mouse = new Point((int)pointToCheck.X, (int)pointToCheck.Y);
            return Border.Contains(mouse);
        }

        private InputListenerManager inputManager;

        private Vector2 origin;

        public Card(SpriteBatch sb, Texture2D texture, Vector2 position, int value) {
            spriteBatch = sb;
            Texture = texture;
            Position = position;
            origin = position;
            stackValue = value;
        }

        public void Draw(GameTime gameTime) {
            Color colorToUse = Color.White;


            // uncomment below to add mouseover hover coloring
            /* 
            if (IsSelected) {
                colorToUse = Color.Orange;
            }
            else {
                if (IsMouseOver) { colorToUse = Color.Cyan; }
            }
            */

            spriteBatch.Draw(Texture, Position, colorToUse);
        }


        

        public void AddChild(Card child) {



        }



        public Rectangle Border {
            get {

                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            }
        }

        public bool HasChild {

            get {

                if (Child != null) return true;
                else return false;

            }

        }
        
    }

}

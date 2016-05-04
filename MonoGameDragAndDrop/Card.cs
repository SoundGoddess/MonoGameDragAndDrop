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

    class Card : IDragAndDropItem {

        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Card Child { get; }
        public bool IsDraggable { get; set; } = true;
        public ZOrder ZIndex { get; set; } = ZOrder.Normal;
        public int stackValue;

        private Vector2 origin;
        private bool returnToOrigin = false;

        private readonly SpriteBatch spriteBatch;

        public bool IsSelected { get; set; }
        public bool IsMouseOver { get; set; }
        public bool Contains(Vector2 pointToCheck) {
            Point mouse = new Point((int)pointToCheck.X, (int)pointToCheck.Y);
            return Border.Contains(mouse);
        }


        public void OnSelected() {

            IsSelected = true;
            ZIndex = ZOrder.InFront;

        }

        public void OnDeselected() {

            IsSelected = false;
            returnToOrigin = true;

        }

        private bool ReturnToOrigin() {

            bool backAtOrigin = false;

            var pos = Position;
            float speed = 25.0f;

            float distance = (float)Math.Sqrt(Math.Pow(origin.X - pos.X, 2) + (float)Math.Pow(origin.Y - pos.Y, 2));
            float directionX = (origin.X - pos.X) / distance;
            float directionY = (origin.Y - pos.Y) / distance;

            pos.X += directionX * speed;
            pos.Y += directionY * speed;

            
            if (Math.Sqrt(Math.Pow(pos.X - Position.X, 2) + Math.Pow(pos.Y - Position.Y, 2)) >= distance) { 

                Position = origin;

                backAtOrigin = true;

                ZIndex = ZOrder.Normal;
                
            }
            else Position = pos;

            return backAtOrigin;

        }


        private InputListenerManager inputManager;


        public Card(SpriteBatch sb, Texture2D texture, Vector2 position, int value) {
            spriteBatch = sb;
            Texture = texture;
            Position = position;
            origin = position;
            stackValue = value;
        }

        public void Update(GameTime gameTime) {

            if (returnToOrigin) {

                returnToOrigin = !ReturnToOrigin();

            }


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

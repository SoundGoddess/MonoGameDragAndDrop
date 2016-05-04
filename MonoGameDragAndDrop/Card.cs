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
        public Card Parent { get; set; }
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

            if (Position != origin) returnToOrigin = true;

        }

        public void HandleCollusion(IDragAndDropItem item) {

            Card subCard = (Card)item;

            subCard.AttachToParent(this);
            item = subCard;

            Console.WriteLine("todo: attach card " + stackValue + " to parent " + subCard.stackValue);

        }

        /// <summary>
        /// Animation for returning the card to its original position if it can't find a new place to snap to
        /// </summary>
        /// <returns>returns true if the card is back in its original position; otherwise it increments the animation</returns>
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


        

        public void AttachToParent(Card parent) {
            
            Parent = parent;
            var pos = Position;

            /*
            pos.Y = parent.Position.Y + 20;

            Position = pos;
            origin = pos;
            */

        }



        public Rectangle Border {
            get {

                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            }
        }

        public bool HasParent {

            get {

                if (Parent != null) return true;
                else return false;

            }

        }
        
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


    class Item : Game {

        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Item Child { get; }
        public bool IsDraggable { get; set; } = true;
        public ZOrder ZIndex { get; set; } = ZOrder.Normal;

        private InputListenerManager inputManager;

        private Vector2 origin;

        public Item(Texture2D texture, Vector2 position) {
            Texture = texture;
            Position = position;
            origin = position;
        }


        public void OnDrag() {

            if (IsDraggable) {

                ZIndex = ZOrder.InFront;

                var mouseState = Mouse.GetState();
                Position = new Vector2(mouseState.X, mouseState.Y);

                Console.WriteLine("move this:" + mouseState.X, mouseState.Y);

            }

        }


        public void OnDrop() {

            // if it's supposed to stick to wherever it lands then update origin
            // otherwise do an animation to return it to origin


        }



        public void AddChild(Item child) {



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

        protected override void Initialize() {

            inputManager = new InputListenerManager();
            var mouseListener = inputManager.AddListener(new MouseListenerSettings());

            mouseListener.MouseDrag += (sender, args) => OnDrag();


            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {


            inputManager.Update(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime) {


            base.Draw(gameTime);

        }
    }

}

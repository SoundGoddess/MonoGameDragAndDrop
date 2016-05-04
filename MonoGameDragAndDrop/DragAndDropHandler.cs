using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDragAndDrop {

    class DragAndDropHandler<T> : DrawableGameComponent where T : IDragAndDropItem {

        MouseState oldMouse, currentMouse;
        SpriteBatch spriteBatch;
        private readonly List<T> _items;
        private readonly List<T> _selectedItems;

        public T ItemUnderTheMouseCursor { get; private set; }
        public bool IsThereAnItemUnderTheMouseCursor { get; private set; }

        public IEnumerable<T> Items { get { foreach (var item in _items) { yield return item; } } }
        public IEnumerable<T> SelectedItems { get { foreach (var item in _selectedItems) { yield return item; } } }

        public int Count { get { return _items.Count; } }


        private bool MouseWasJustPressed {
            get {
                return currentMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released;
            }
        }

        private Vector2 CurrentMousePosition {
            get { return new Vector2(currentMouse.X, currentMouse.Y); }
        }

        public Vector2 OldMousePosition {
            get { return new Vector2(oldMouse.X, oldMouse.Y); }
        }

        public Vector2 MouseMovementSinceLastUpdate {
            get { return CurrentMousePosition - OldMousePosition; }
        }

        private void SaveCurrentMouseState() {
            oldMouse = currentMouse;
        }

        public T ItemUnderMouseCursor() {
            for (int i = _items.Count - 1; i >= 0; i--) {
                if (_items[i].Contains(CurrentMousePosition)) {
                    return _items[i];
                }
            }
            return default(T);
        }


        public DragAndDropHandler(Game game, SpriteBatch sb) : base(game) {
            spriteBatch = sb;
            _selectedItems = new List<T>();
            _items = new List<T>();
        }

        public override void Update(GameTime gameTime) {
            GetCurrentMouseState();
            HandleMouseInput();
            SaveCurrentMouseState();
        }


        private void HandleMouseInput() {

            SetAllItemsToIsMouseOverFalse();

            ItemUnderTheMouseCursor = ItemUnderMouseCursor();

            if (!Equals(ItemUnderTheMouseCursor, default(T))) {
                UpdateItemUnderMouse();
            }
            else {
                if (MouseWasJustPressed) {
                    DeselectAll();
                }
            }

            if (currentMouse.LeftButton != ButtonState.Released) MoveSelectedItemsIfMouseButtonIsPressed();
            else DeselectAll();
        }


        public void DeselectAll() {
            for (int i = _selectedItems.Count - 1; i >= 0; i--) {
                DeselectItem(_selectedItems[i]);
            }
        }

        private void SetAllItemsToIsMouseOverFalse() {
            _items.ForEach(item => item.IsMouseOver = false);
        }


        private void MoveSelectedItemsIfMouseButtonIsPressed() {
            if (currentMouse.LeftButton == ButtonState.Pressed) {
                foreach (T item in SelectedItems) {

                    if (item.IsDraggable) item.Position += MouseMovementSinceLastUpdate;

                }
           }
        }

        private void UpdateItemUnderMouse() {
            ItemUnderTheMouseCursor.IsMouseOver = true;

            if (MouseWasJustPressed) {
                SelectItem(ItemUnderTheMouseCursor);
            }
        }


        private void SelectItem(T itemToSelect) {
            itemToSelect.IsSelected = true;
            itemToSelect.ZIndex = ZOrder.InFront;
            if (!_selectedItems.Contains(itemToSelect)) {
                _selectedItems.Add(itemToSelect);
            }
        }

        private void DeselectItem(T itemToDeselect) {
            itemToDeselect.IsSelected = false;
            itemToDeselect.ZIndex = ZOrder.Normal;
            _selectedItems.Remove(itemToDeselect);
        }

        private void GetCurrentMouseState() { currentMouse = Mouse.GetState(); }


        public void Add(T item) { _items.Add(item); }
        public void Remove(T item) { _items.Remove(item); _selectedItems.Remove(item); }

        public void Clear() {
            _selectedItems.Clear();
            _items.Clear();
        }

    }
}

﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.ViewportAdapters;

namespace MonoGameDragAndDrop {

    class DragAndDropHandler<T> : DrawableGameComponent where T : IDragAndDropItem {

        MouseState oldMouse, currentMouse;
        SpriteBatch spriteBatch;
        ScalingViewportAdapter viewport;

        private List<T> _items;
        private readonly List<T> _selectedItems;

        public T ItemUnderTheMouseCursor { get; private set; }
        public bool IsThereAnItemUnderTheMouseCursor { get; private set; }

        public IEnumerable<T> Items {
            get {

                // since MonoGame renders sprites on top of each other based on the order they are called in the Draw() method, this
                // little line of code sorts the sprite objects to take into consideration the ZIndex so that things render as expected.
                _items = _items.OrderBy(z => z.ZIndex).ToList();
                foreach (var item in _items) { yield return item; }

            }
        }
        public IEnumerable<T> SelectedItems { get { foreach (var item in _selectedItems) { yield return item; } } }

        public int Count { get { return _items.Count; } }


        private bool MouseWasJustPressed {
            get {
                return currentMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released;
            }
        }
        private bool MouseWasJustUnpressed {
            get {
                return currentMouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed;
            }
        }

        private Vector2 CurrentMousePosition {
            get {

                Point point = viewport.PointToScreen(currentMouse.X, currentMouse.Y);

                return new Vector2(point.X, point.Y);
                    
            }
        }

        public Vector2 OldMousePosition {
            get {

                Point point = viewport.PointToScreen(oldMouse.X, oldMouse.Y);

                return new Vector2(point.X, point.Y);
                

            }
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


        public DragAndDropHandler(Game game, SpriteBatch sb, ScalingViewportAdapter vp) : base(game) {
            spriteBatch = sb;
            viewport = vp;
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

                if (MouseWasJustUnpressed) {

                    _items = _items.OrderBy(z => z.ZIndex).ToList();

                    foreach (T item in _items) {

                        if (!Equals(ItemUnderTheMouseCursor, item)) {

                            ItemUnderTheMouseCursor.HandleCollusion(item);

                        }

                    }


                }
            }
            else {
                if (MouseWasJustPressed) {
                    DeselectAll();
                }
            }



            if (currentMouse.LeftButton != ButtonState.Released) MoveSelectedItemsIfMouseButtonIsPressed();
            else DeselectAll();
        }

        public T SubItemUnderMouseCursor(T currentItem) {

            _items = _items.OrderBy(z => z.ZIndex).ToList();

            foreach (var item in _items) {

                if (item.Contains(CurrentMousePosition)) return item;
            }
            return default(T);
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
            itemToSelect.OnSelected();
            if (!_selectedItems.Contains(itemToSelect)) {
                _selectedItems.Add(itemToSelect);
            }
        }

        private void DeselectItem(T itemToDeselect) {
            itemToDeselect.OnDeselected();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDragAndDrop {
    /// <summary>
    /// Interface describing necessary implementation for working with the DragAndDropHandler.
    /// </summary>
    public interface IDragAndDropItem {
        Vector2 Position { get; set; }
        bool IsSelected { get; set; }
        bool IsMouseOver { set; }
        bool Contains(Vector2 pointToCheck);
        Rectangle Border { get; }
        bool IsDraggable { get; set; }
        //ZOrder ZIndex { get; set; }
        int ZIndex { get; set; }
        Texture2D Texture { get; set; }

        void OnSelected();
        void OnDeselected();
        void HandleCollusion(IDragAndDropItem item);
    }
}
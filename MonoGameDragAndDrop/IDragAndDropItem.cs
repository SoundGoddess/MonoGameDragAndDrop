using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MonoGameDragAndDrop {
    /// <summary>
    /// Interface describing necessary implementation for working with the DragAndDropHandler.
    /// </summary>
    public interface IDragAndDropItem {
        Vector2 Position { get; set; }
        bool IsSelected { set; }
        bool IsMouseOver { set; }
        bool Contains(Vector2 pointToCheck);
        Rectangle Border { get; }
        bool IsDraggable { get; set; }
        ZOrder ZIndex { get; set; }

        void OnSelected();
        void OnDeselected();
    }
}
using UnityEngine;
using UnityEngine.UIElements;

namespace Conversa.Editor
{
    public static class Icons
    {
        public static Image Question => new Image
        {
            image = Resources.Load("question") as Texture2D,
            scaleMode = ScaleMode.ScaleToFit,
            pickingMode = PickingMode.Ignore
        };

        public static Image Bookmark() => new Image
        {
            image = Resources.Load("bookmark") as Texture2D,
            scaleMode = ScaleMode.ScaleToFit,
            pickingMode = PickingMode.Ignore
        };
        
        public static Image Arrow() => new Image
        {
            image = Resources.Load("arrow-right") as Texture2D,
            scaleMode = ScaleMode.ScaleToFit,
            pickingMode = PickingMode.Ignore
        };

    }
}
using OpenTK.Input;

namespace DynaStudios.UI.Controls
{
    public interface IUiControl : IUIPlacable
    {
        /// <summary>
        /// Renders the UIControl
        /// </summary>
        void Render();

        /// <summary>
        /// Recalculates Width and Height Dimensions
        /// </summary>
        void Resize();

        /// <summary>
        /// Called when the UIControl got clicked
        /// </summary>
        /// <param name="arg">Mouse Informations</param>
        void OnClicked(MouseButtonEventArgs arg);

        /// <summary>
        /// Called when the UIControl received Keyboard Input
        /// </summary>
        /// <param name="arg"></param>
        void OnKeyPressed(KeyboardKeyEventArgs arg);

        /// <summary>
        /// Called when Mouse Enters UIControl area
        /// </summary>
        void OnHoverEnter();

        /// <summary>
        /// Called when Mouse Leaves UIControl area
        /// </summary>
        void OnHoverExit();
    }
}
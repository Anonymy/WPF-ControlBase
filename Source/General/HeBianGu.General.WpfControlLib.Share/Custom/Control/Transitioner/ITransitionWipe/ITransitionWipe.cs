using System.Windows;

namespace HeBianGu.General.WpfControlLib
{
    /// <summary> ����Ч�� </summary>
    public interface ITransitionWipe
    {
        void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController);
    }
}
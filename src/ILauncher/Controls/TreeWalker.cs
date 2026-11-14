using System.Windows;
using System.Windows.Media;

namespace ILauncher.Controls;

/// <summary>
/// 视觉/逻辑树查找工具。WPF 里 <c>Run</c>、<c>Hyperlink</c> 等 <see cref="System.Windows.Documents.Run"/>
/// 属于内容元素（不是 Visual），直接调用 <see cref="VisualTreeHelper.GetParent"/> 会抛
/// “不是 Visual 或 Visual3D”。这里统一做类型判断，事件处理里可以放心用 OriginalSource 往上找。
/// </summary>
public static class TreeWalker
{
    public static T? FindAncestor<T>(DependencyObject? current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T match)
            {
                return match;
            }

            current = GetParent(current);
        }

        return null;
    }

    public static DependencyObject? GetParent(DependencyObject current)
    {
        try
        {
            return current is Visual or System.Windows.Media.Media3D.Visual3D
                ? VisualTreeHelper.GetParent(current)
                : LogicalTreeHelper.GetParent(current);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}

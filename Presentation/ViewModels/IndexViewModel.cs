namespace Presentation.ViewModels;

public record IndexViewModel<T>
{
    public IEnumerable<T> Items { get; init; }
    public PageViewModel PageViewModel { get; init; }
}
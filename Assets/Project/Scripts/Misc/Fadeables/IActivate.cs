namespace Misc
{
    public interface IActivate
    {
        bool IsActive { get; }
        void SetActive(bool value);
    }
}
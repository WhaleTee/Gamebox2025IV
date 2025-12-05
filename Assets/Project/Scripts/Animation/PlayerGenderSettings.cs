using Misc;

namespace Animation
{
    public class PlayerGenderSettings : Singleton<PlayerGenderSettings>
    {
        public Gender Gender { get; private set; }
        
        public void SetFemaleController() => Gender = Gender.Female;
        
        public void SetMaleController() => Gender = Gender.Male;
    }
}
namespace Pr11.Models.Bosses
{
    public class Archmage : Mage
    {
        public Archmage()
        {
            Name = "Архимаг C++";
            HP = (int)(HP * 1.8);
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
        }
    }
}
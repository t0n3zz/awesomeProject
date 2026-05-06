namespace Pr16.Models
{
    public class VVG : Goblin
    {
        public VVG()
        {
            Name = "ВВГ";
            HP = 60;
            Attack = 18;
            Defense = 3;
        }

        public override int CritChance => 30;
    }
}

namespace Pr16.Models
{
    public class PestovMinusMinus : Skeleton
    {
        public PestovMinusMinus()
        {
            Name = "Пестов С--";
            HP = 52;
            Attack = 18;
            Defense = 3;
        }

        public override int FreezeChance => 15;
        public override bool IgnoresArmor => true;
    }
}

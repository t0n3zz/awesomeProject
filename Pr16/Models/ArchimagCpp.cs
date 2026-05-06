namespace Pr16.Models
{
    public class ArchimagCpp : Mage
    {
        public ArchimagCpp()
        {
            Name = "Архимаг С++";
            HP = 45;
            Attack = 24;
            Defense = 2;
        }

        public override int FreezeChance => 25;
    }
}

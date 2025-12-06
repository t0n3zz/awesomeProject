namespace Pr11.Models.Bosses
{
    public class VVG : Goblin
    {
        public VVG()
        {
            Name = "ВВГ";
            HP = (int)(HP * 2.0);
            Attack = (int)(Attack * 1.5);
            Defense = (int)(Defense * 1.2);
        }
    }
}
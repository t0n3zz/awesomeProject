namespace Pr16.Models
{
    public class Armor : Item
    {
        public int Defense { get; set; }

        public override string ToString()
        {
            return $"{Name} (+{Defense} защиты)";
        }
    }
}

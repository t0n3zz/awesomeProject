namespace Pr16.Models
{
    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int HP { get; protected set; }
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public virtual int CritChance => 0;
        public virtual int FreezeChance => 0;
        public virtual bool IgnoresArmor => false;

        public bool IsAlive => HP > 0;

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                damage = 0;

            HP -= damage;

            if (HP < 0)
                HP = 0;
        }
    }
}

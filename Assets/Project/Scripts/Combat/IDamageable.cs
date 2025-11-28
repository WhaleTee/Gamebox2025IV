namespace Combat
{
    public interface IDamageable
    {
        public void SetDamageSource(IDamageSource damageSource);
        public void InflictDamage(DamageBundle damage);
    }
}
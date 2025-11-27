using System;
using System.Collections.Generic;
using UnityEngine;
using Combat;

[Serializable]
public class DamageBundle
{
    public Dictionary<DamageType, int> Damage => LazyDict();
    [SerializeField] private DamageType[] m_types;
    [SerializeField] private int[] m_amounts;
    private Dictionary<DamageType, int> damages = new();

    public bool Has(DamageType type) => damages.ContainsKey(type);

    public int Get(DamageType type)
    {
        if (damages == null)
            Create();

        damages.TryGetValue(type, out int v);
        return v;
    }

    private Dictionary<DamageType, int> LazyDict()
    {
        if (damages == null)
        {
            Create();
            DisposeUnnecessary();
        }
        return damages;
    }

    private void Create()
    {
        if (m_types == null || m_amounts == null)
        {
            damages = new Dictionary<DamageType, int>();
            return;
        }

        int len = Mathf.Min(m_types.Length, m_amounts.Length);
        damages = new Dictionary<DamageType, int>(len);
        for (int i = 0; i < len; i++)
        {
            damages[m_types[i]] = m_amounts[i];
        }
    }

    private void DisposeUnnecessary()
    {
        m_types = null;
        m_amounts = null;
    }
}
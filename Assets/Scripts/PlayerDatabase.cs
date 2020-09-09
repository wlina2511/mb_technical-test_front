using System.Collections.Generic;
using UnityEngine;


// Our Player databse with getters and "setters" (Add and Clear methods)

[CreateAssetMenu]
public class PlayerDatabase : ScriptableObject
{
    [SerializeField]
    private List<Player> database = new List<Player>();

    public List<Player> GetPlayers() => database;

    public void Add(Player player)
    {
        database.Add(player);
    }

    public void ClearDatabase()
    {
        database.Clear();
    }
}

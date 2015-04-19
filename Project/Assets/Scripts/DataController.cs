using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataController : MonoBehaviour {

    public static DataController dataController;

    //general state of player
    public int level = 1;
    public int coins = 0;
    public int crystals = 3;

    //general upgrades
    public int p_cave = 100;
    public int m_scream = 12;
    public int m_earthquake = 50;

    //fire upgrades
    public int b_fireDamage = 50;
    public int b_fireRange = 10;
    public int b_fireHeavnlyFire = 50;
    public int b_fireThunder = 50;
    public int p_fireLava = 50;
    public int m_fireMeteor = 50;

    //air upgrades
    public int b_airDamage = 50;
    public int b_airRange = 10;
    public int b_airSkyFall = 50;
    public int b_airCursedBreath = 50;
    public int p_airTornado = 50;
    public int m_airTornado = 50;

    //water upgrades
    public int b_waterDamage = 50;
    public int b_waterRange = 10;
    public int b_waterFrozenSky = 50;
    public int b_waterCursedBreath = 50;
    public int p_waterIceWall = 50;
    public int m_waterMist = 50;

    public DragonAttack.AttackType attackType;

    //info to pass from level to victory/lose scene
    public int kills = 0;
    public int life = 0;
    public int coinsFromStage = 0;
    public int crystalsFromStage = 0;
    public bool won = false;

	void Awake () {
        if (dataController == null)
        {
            DontDestroyOnLoad(gameObject);
            dataController = this;
        }
        else if (dataController != this)
        {
            Destroy(gameObject);
        }
	}

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream playerDataFile = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.level = level;
        data.coins = coins;
        data.crystals = crystals;

        data.p_cave = p_cave;
        data.m_scream = m_scream;
        data.m_earthquake = m_earthquake;

        data.b_fireDamage = b_fireDamage;
        data.b_fireRange = b_fireRange;
        data.b_fireHeavnlyFire = b_fireHeavnlyFire;
        data.b_fireThunder = b_fireThunder;
        data.p_fireLava = p_fireLava;
        data.m_fireMeteor = m_fireMeteor;

        data.b_airDamage = b_airDamage;
        data.b_airRange = b_airRange;
        data.b_airSkyFall = b_airSkyFall;
        data.b_airCursedBreath = b_airCursedBreath;
        data.p_airTornado = p_airTornado;
        data.m_airTornado = m_airTornado;

        data.b_waterDamage = b_waterDamage;
        data.b_waterRange = b_waterRange;
        data.b_waterFrozenSky = b_waterFrozenSky;
        data.b_waterCursedBreath = b_waterCursedBreath;
        data.p_waterIceWall = p_waterIceWall;
        data.m_waterMist = m_waterMist;

        data.attackType = attackType;

        binaryFormatter.Serialize(playerDataFile, data);
        playerDataFile.Close();

    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream playerDataFile = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData) binaryFormatter.Deserialize(playerDataFile);
            playerDataFile.Close();

            p_cave = data.p_cave;
            m_scream = data.m_scream;
            m_earthquake = data.m_earthquake;

            b_fireDamage = data.b_fireDamage;
            b_fireRange = data.b_fireRange;
            b_fireHeavnlyFire = data.b_fireHeavnlyFire;
            b_fireThunder = data.b_fireThunder;
            p_fireLava = data.p_fireLava;
            m_fireMeteor = data.m_fireMeteor;

            b_airDamage = data.b_airDamage;
            b_airRange = data.b_airRange;
            b_airSkyFall = data.b_airSkyFall;
            b_airCursedBreath = data.b_airCursedBreath;
            p_airTornado = data.p_airTornado;
            m_airTornado = data.m_airTornado;

            b_waterDamage = data.b_waterDamage;
            b_waterRange = data.b_waterRange;
            b_waterFrozenSky = data.b_waterFrozenSky;
            b_waterCursedBreath = data.b_waterCursedBreath;
            p_waterIceWall = data.p_waterIceWall;
            m_waterMist = data.m_waterMist;

            attackType = data.attackType;
        }
    }
}

[Serializable]
class PlayerData
{
    public int level;
    public int coins;
    public int crystals;

    public int p_cave;
    public int m_scream;
    public int m_earthquake;

    public int b_fireDamage;
    public int b_fireRange;
    public int b_fireHeavnlyFire;
    public int b_fireThunder;
    public int p_fireLava;
    public int m_fireMeteor;

    public int b_airDamage;
    public int b_airRange;
    public int b_airSkyFall;
    public int b_airCursedBreath;
    public int p_airTornado;
    public int m_airTornado;

    public int b_waterDamage;
    public int b_waterRange;
    public int b_waterFrozenSky;
    public int b_waterCursedBreath;
    public int p_waterIceWall;
    public int m_waterMist;

    public DragonAttack.AttackType attackType;

}


//class AttackTypeDefinition
//{
  //  public enum AttackTypeDefinition { Air, Fire, Water };
//
  //  public int damage;
    //public int range;
   // public int firstBreathSpecial;
   // public int secondBreathSpecial;

   // public int cave;
   // public int protectSpecial;

  //  public int scream;
    //public int magicSpecial;
//    public int earthquake;

  //  public AttackTypeDefinition(AttackTypeDefinition type)
    //{

    //}
//}

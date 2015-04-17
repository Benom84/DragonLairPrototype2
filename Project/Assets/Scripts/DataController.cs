using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataController : MonoBehaviour {

    public static DataController dataController;

    public int level;
    public int coins;
    public int crystals;

    public int b_fireDamage;
    public int b_fireRange;
    public int b_fireHeavnlyFire;
    public int b_fireThunder;
    public int p_fireCave;
    public int p_fireLava;
    public int m_fireScream;
    public int m_fireMeteor;
    public int m_fireEarthquake;

    public int b_airDamage;
    public int b_airRange;
    public int b_airSkyFall;
    public int b_airCursedBreath;
    public int p_airCave;
    public int p_airTornado;
    public int m_airScream;
    public int m_airTornado;
    public int m_airEarthquake;

    public int b_waterDamage;
    public int b_waterRange;
    public int b_waterFrozenSky;
    public int b_waterCursedBreath;
    public int p_waterCave;
    public int p_waterIceWall;
    public int m_waterScream;
    public int m_waterMist;
    public int m_waterEarthquake;

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

        data.b_fireDamage = b_fireDamage;
        data.b_fireRange = b_fireRange;
        data.b_fireHeavnlyFire = b_fireHeavnlyFire;
        data.b_fireThunder = b_fireThunder;
        data.p_fireCave = p_fireCave;
        data.p_fireLava = p_fireLava;
        data.m_fireScream = m_fireScream;
        data.m_fireMeteor = m_fireMeteor;
        data.m_fireEarthquake = m_fireEarthquake;

        data.b_airDamage = b_airDamage;
        data.b_airRange = b_airRange;
        data.b_airSkyFall = b_airSkyFall;
        data.b_airCursedBreath = b_airCursedBreath;
        data.p_airCave = p_airCave;
        data.p_airTornado = p_airTornado;
        data.m_airScream = m_airScream;
        data.m_airTornado = m_airTornado;
        data.m_airEarthquake = m_airEarthquake;

        data.b_waterDamage = b_waterDamage;
        data.b_waterRange = b_waterRange;
        data.b_waterFrozenSky = b_waterFrozenSky;
        data.b_waterCursedBreath = b_waterCursedBreath;
        data.p_waterCave = p_waterCave;
        data.p_waterIceWall = p_waterIceWall;
        data.m_waterScream = m_waterScream;
        data.m_waterMist = m_waterMist;
        data.m_waterEarthquake = m_waterEarthquake;

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

            b_fireDamage = data.b_fireDamage;
            b_fireRange = data.b_fireRange;
            b_fireHeavnlyFire = data.b_fireHeavnlyFire;
            b_fireThunder = data.b_fireThunder;
            p_fireCave = data.p_fireCave;
            p_fireLava = data.p_fireLava;
            m_fireScream = data.m_fireScream;
            m_fireMeteor = data.m_fireMeteor;
            m_fireEarthquake = data.m_fireEarthquake;

            b_airDamage = data.b_airDamage;
            b_airRange = data.b_airRange;
            b_airSkyFall = data.b_airSkyFall;
            b_airCursedBreath = data.b_airCursedBreath;
            p_airCave = data.p_airCave;
            p_airTornado = data.p_airTornado;
            m_airScream = data.m_airScream;
            m_airTornado = data.m_airTornado;
            m_airEarthquake = data.m_airEarthquake;

            b_waterDamage = data.b_waterDamage;
            b_waterRange = data.b_waterRange;
            b_waterFrozenSky = data.b_waterFrozenSky;
            b_waterCursedBreath = data.b_waterCursedBreath;
            p_waterCave = data.p_waterCave;
            p_waterIceWall = data.p_waterIceWall;
            m_waterScream = data.m_waterScream;
            m_waterMist = data.m_waterMist;
            m_waterEarthquake = data.m_waterEarthquake;
        }
    }
}

[Serializable]
class PlayerData
{
    public int level;
    public int coins;
    public int crystals;

    public int b_fireDamage;
    public int b_fireRange;
    public int b_fireHeavnlyFire;
    public int b_fireThunder;
    public int p_fireCave;
    public int p_fireLava;
    public int m_fireScream;
    public int m_fireMeteor;
    public int m_fireEarthquake;

    public int b_airDamage;
    public int b_airRange;
    public int b_airSkyFall;
    public int b_airCursedBreath;
    public int p_airCave;
    public int p_airTornado;
    public int m_airScream;
    public int m_airTornado;
    public int m_airEarthquake;

    public int b_waterDamage;
    public int b_waterRange;
    public int b_waterFrozenSky;
    public int b_waterCursedBreath;
    public int p_waterCave;
    public int p_waterIceWall;
    public int m_waterScream;
    public int m_waterMist;
    public int m_waterEarthquake;
}

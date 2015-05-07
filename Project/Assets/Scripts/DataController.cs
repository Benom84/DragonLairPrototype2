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
    public int m_screamData = 12;
    public int m_tailData = 50; //earthquake or tail?
    public int p_caveData = 50;
    public int b_fireDamageData = 50;
    public float b_fireAgilityData = 0.5f;
    public int b_waterDamageData = 50;
    public float b_waterAgilityData = 0.5f;
    public int b_heavenlyFireData = 50;
    public int b_frozenSkyData = 50;
    public int b_thunderData = 50;
    public int b_cursedBreathData = 50;
    public int m_meteorData = 50;
    public int m_iceData = 50;
    public int m_manaData = 100;

    public int m_screamLevel = 0;
    public int m_tailLevel = 0; //earthquake or tail?
    public int p_caveLevel = 0;
    public int b_fireDamageLevel = 1;
    public int b_fireAgilityLevel = 1;
    public int b_waterDamageLevel = 0;
    public int b_waterAgilityLevel = 0;
    public int b_heavenlyFireLevel = 0;
    public int b_frozenSkyLevel = 0;
    public int b_thunderLevel = 0;
    public int b_cursedBreathLevel = 0;
    public int m_meteorLevel = 0;
    public int m_iceLevel = 0;
    public int m_manaLevel = 0;

    public int[] upgradesLevel;
    public float[] upgradesData;

    public bool isWaterUnlocked;

    //info to pass from level to victory/lose scene
    public int kills = 0;
    public int life = 0;
    public int coinsFromStage = 0;
    public int bonusCoinsFromStage = 0;
    public int crystalsFromStage = 0;
    public bool won = false;

    public int delayBetweenDragonBreath = 100;
    public int screamManaValue = 20;
    public int meteorManaValue = 20;
    public int iceManaValue = 20;
    public int earthquakeManaValue = 20;

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

        upgradesLevel = new int[] { b_fireDamageLevel, b_fireAgilityLevel, b_waterDamageLevel, b_waterAgilityLevel, b_heavenlyFireLevel, b_frozenSkyLevel, b_thunderLevel, b_cursedBreathLevel,
            p_caveLevel, m_screamLevel, m_meteorLevel, m_iceLevel, m_tailLevel, m_manaLevel };

        upgradesData = new float[] { b_fireDamageData, b_fireAgilityData, b_waterDamageData, b_waterAgilityData, b_heavenlyFireData, b_frozenSkyData, b_thunderData, b_cursedBreathData, 
            p_caveData, m_screamData, m_meteorData, m_iceData, m_tailData, m_manaData };

        if (level > 4)
        {
            isWaterUnlocked = true;
        }
        else
        {
            isWaterUnlocked = false;
        }
	}

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream playerDataFile;
//        if (!File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
  //      {
            playerDataFile = File.Create(Application.persistentDataPath + "/playerInfo.dat");
    //    }
      //  else
        ///{
           // playerDataFile = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
       // }
        

        PlayerData data = new PlayerData();
        data.level = level;
        data.coins = coins;
        data.crystals = crystals;

        data.b_fireDamageLevel = b_fireDamageLevel;
        data.b_fireAgilityLevel = b_fireAgilityLevel;
        data.b_waterDamageLevel = b_waterDamageLevel;
        data.b_waterAgilityLevel = b_waterAgilityLevel;
        data.b_heavenlyFireLevel = b_heavenlyFireLevel;
        data.b_frozenSkyLevel = b_frozenSkyLevel;
        data.b_thunderLevel = b_thunderLevel;
        data.b_cursedBreathLevel = b_cursedBreathLevel;
        data.p_caveLevel = p_caveLevel;
        data.m_screamLevel = m_screamLevel;
        data.m_meteorLevel = m_meteorLevel;
        data.m_iceLevel = m_iceLevel;
        data.m_tailLevel = m_tailLevel;
        data.m_manaLevel = m_manaLevel;

        data.b_fireDamageData = b_fireDamageData;
        data.b_fireAgilityData = b_fireAgilityData;
        data.b_waterDamageData = b_waterDamageData;
        data.b_waterAgilityData = b_waterAgilityData;
        data.b_heavenlyFireData = b_heavenlyFireData;
        data.b_frozenSkyData = b_frozenSkyData;
        data.b_thunderData = b_thunderData;
        data.b_cursedBreathData = b_cursedBreathData;
        data.p_caveData = p_caveData;
        data.m_screamData = m_screamData;
        data.m_meteorData = m_meteorData;
        data.m_iceData = m_iceData;
        data.m_tailData = m_tailData;
        data.m_manaData = m_manaData;


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

            level = data.level;
            coins = data.coins;
            crystals = data.crystals;

            b_fireDamageLevel = data.b_fireDamageLevel;
            b_fireAgilityLevel = data.b_fireAgilityLevel;
            b_waterDamageLevel = data.b_waterDamageLevel;
            b_waterAgilityLevel = data.b_waterAgilityLevel;
            b_heavenlyFireLevel = data.b_heavenlyFireLevel;
            b_frozenSkyLevel = data.b_frozenSkyLevel;
            b_thunderLevel = data.b_thunderLevel;
            b_cursedBreathLevel = data.b_cursedBreathLevel;
            p_caveLevel = data.p_caveLevel;
            m_screamLevel = data.m_screamLevel;
            m_meteorLevel = data.m_meteorLevel;
            m_iceLevel = data.m_iceLevel;
            m_tailLevel = data.m_tailLevel;
            m_manaLevel = data.m_manaLevel;

            b_fireDamageData = data.b_fireDamageData;
            b_fireAgilityData = data.b_fireAgilityData;
            b_waterDamageData = data.b_waterDamageData;
            b_waterAgilityData = data.b_waterAgilityData;
            b_heavenlyFireData = data.b_heavenlyFireData;
            b_frozenSkyData = data.b_frozenSkyData;
            b_thunderData = data.b_thunderData;
            b_cursedBreathData = data.b_cursedBreathData;
            p_caveData = data.p_caveData;
            m_screamData = data.m_screamData;
            m_meteorData = data.m_meteorData;
            m_iceData = data.m_iceData;
            m_tailData = data.m_tailData;
            m_manaData = data.m_manaData;

        }
    }
}

[Serializable]
class PlayerData
{
    public int level;
    public int coins;
    public int crystals;

    public int m_screamData;
    public int m_tailData; //earthquake or tail?
    public int p_caveData;
    public int b_fireDamageData;
    public float b_fireAgilityData;
    public int b_waterDamageData;
    public float b_waterAgilityData;
    public int b_heavenlyFireData;
    public int b_frozenSkyData;
    public int b_thunderData;
    public int b_cursedBreathData;
    public int m_meteorData;
    public int m_iceData;
    public int m_manaData;

    public int m_screamLevel;
    public int m_tailLevel; //earthquake or tail?
    public int p_caveLevel;
    public int b_fireDamageLevel;
    public int b_fireAgilityLevel;
    public int b_waterDamageLevel;
    public int b_waterAgilityLevel;
    public int b_heavenlyFireLevel;
    public int b_frozenSkyLevel;
    public int b_thunderLevel;
    public int b_cursedBreathLevel;
    public int m_meteorLevel;
    public int m_iceLevel;
    public int m_manaLevel;
}
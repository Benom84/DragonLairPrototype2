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
    public int m_earthquakeData = 50; //earthquake or tail?
    public int p_caveData = 50;
    public int b_damageData = 50;
    public int b_agilityData = 50;
    public int b_heavenlyFireData = 50;
    public int b_frozenSkyData = 50;
    public int b_thunderData = 50;
    public int b_cursedBreathData = 50;
    public int m_meteorData = 50;
    public int m_iceData = 50;

    public int m_screamLevel = 0;
    public int m_earthquakeLevel = 0; //earthquake or tail?
    public int p_caveLevel = 0;
    public int b_damageLevel = 1;
    public int b_agilityLevel = 1;
    public int b_heavenlyFireLevel = 0;
    public int b_frozenSkyLevel = 0;
    public int b_thunderLevel = 0;
    public int b_cursedBreathLevel = 0;
    public int m_meteorLevel = 0;
    public int m_iceLevel = 0;

    public int[] upgradesLevel;
    public int[] upgradesData;


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

        upgradesLevel = new int[] { b_damageLevel, b_agilityLevel, b_heavenlyFireLevel, b_frozenSkyLevel, b_thunderLevel, b_cursedBreathLevel,
            p_caveLevel, m_screamLevel, m_meteorLevel, m_iceLevel, m_earthquakeLevel };

        upgradesLevel = new int[] { b_damageData, b_agilityData, b_heavenlyFireData, b_frozenSkyData, b_thunderData, b_cursedBreathData,
            p_caveData, m_screamData, m_meteorData, m_iceData, m_earthquakeData };
	}

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream playerDataFile;
        if (!File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            playerDataFile = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        }
        else
        {
            playerDataFile = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
        }
        

        PlayerData data = new PlayerData();
        data.level = level;
        data.coins = coins;
        data.crystals = crystals;

        data.b_damageLevel = b_damageLevel;
        data.b_agilityLevel = b_agilityLevel;
        data.b_heavenlyFireLevel = b_heavenlyFireLevel;
        data.b_frozenSkyLevel = b_frozenSkyLevel;
        data.b_thunderLevel = b_thunderLevel;
        data.b_cursedBreathLevel = b_cursedBreathLevel;
        data.p_caveLevel = p_caveLevel;
        data.m_screamLevel = m_screamLevel;
        data.m_meteorLevel = m_meteorLevel;
        data.m_iceLevel = m_iceLevel;
        data.m_earthquakeLevel = m_earthquakeLevel;
        
        data.b_damageData = b_damageData;
        data.b_agilityData = b_agilityData;
        data.b_heavenlyFireData = b_heavenlyFireData;
        data.b_frozenSkyData = b_frozenSkyData;
        data.b_thunderData = b_thunderData;
        data.b_cursedBreathData = b_cursedBreathData;
        data.p_caveData = p_caveData;
        data.m_screamData = m_screamData;
        data.m_meteorData = m_meteorData;
        data.m_iceData = m_iceData;
        data.m_earthquakeData = m_earthquakeData;

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

            b_damageLevel = data.b_damageLevel;
            b_agilityLevel = data.b_agilityLevel;
            b_heavenlyFireLevel = data.b_heavenlyFireLevel;
            b_frozenSkyLevel = data.b_frozenSkyLevel;
            b_thunderLevel = data.b_thunderLevel;
            b_cursedBreathLevel = data.b_cursedBreathLevel;
            p_caveLevel = data.p_caveLevel;
            m_screamLevel = data.m_screamLevel;
            m_meteorLevel = data.m_meteorLevel;
            m_iceLevel = data.m_iceLevel;
            m_earthquakeLevel = data.m_earthquakeLevel;

            b_damageData = data.b_damageData;
            b_agilityData = data.b_agilityData;
            b_heavenlyFireData = data.b_heavenlyFireData;
            b_frozenSkyData = data.b_frozenSkyData;
            b_thunderData = data.b_thunderData;
            b_cursedBreathData = data.b_cursedBreathData;
            p_caveData = data.p_caveData;
            m_screamData = data.m_screamData;
            m_meteorData = data.m_meteorData;
            m_iceData = data.m_iceData;
            m_earthquakeData = data.m_earthquakeData;

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
    public int m_earthquakeData; //earthquake or tail?
    public int p_caveData;
    public int b_damageData;
    public int b_agilityData;
    public int b_heavenlyFireData;
    public int b_frozenSkyData;
    public int b_thunderData;
    public int b_cursedBreathData;
    public int m_meteorData;
    public int m_iceData;

    public int m_screamLevel;
    public int m_earthquakeLevel; //earthquake or tail?
    public int p_caveLevel;
    public int b_damageLevel;
    public int b_agilityLevel;
    public int b_heavenlyFireLevel;
    public int b_frozenSkyLevel;
    public int b_thunderLevel;
    public int b_cursedBreathLevel;
    public int m_meteorLevel;
    public int m_iceLevel;
}
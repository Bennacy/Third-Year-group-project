using UnityEngine;
using System.IO;

public static class SaveSystem{
    public static readonly string NETWORK_SAVE_FOLDER = Application.dataPath + "/Saves/";

    
    public static void Init(){
        if(!Directory.Exists(NETWORK_SAVE_FOLDER)){
            Directory.CreateDirectory(NETWORK_SAVE_FOLDER);
        }
    }

    public static void Save(string saveString, string fileName, string format = ".json"){
        File.WriteAllText(NETWORK_SAVE_FOLDER + fileName + format, saveString);
    }

    public static void ClearSave(string fileName, string format = ".json"){
        File.WriteAllText(NETWORK_SAVE_FOLDER + fileName + format, "");
    }

    public static bool FileExists(string fileName, string format = ".json"){
        return File.Exists(NETWORK_SAVE_FOLDER + fileName + format);
    }

    public static string Load(string fileName, string format = ".json"){
        if(!FileExists(fileName, format))
            return null;
            
        return File.ReadAllText(NETWORK_SAVE_FOLDER + fileName + format);
    }
}

[System.Serializable] public class PersonalScore{
    public string name;
    public int score;
    public int kills;
    public int time;

    public PersonalScore(int _score, int _time, int _kills, string _name = "Test"){
        score = _score;
        time = _time;
        name = _name;
        kills = _kills;
    }

    public PersonalScore(){
        score = -1;
        time = -1;
        kills = -1;
        name = "NULL";
    }
}

[System.Serializable] public class HighScores{
    public PersonalScore[] scores;

    public HighScores(){
        scores = new PersonalScore[10];
        for(int i = 0; i < 10; i++){
            scores[i] = new PersonalScore();
        }
    }

    public int InsertScore(PersonalScore newScore, int startingIndex = 0){
        for(int i = startingIndex; i < 10; i++){
            if(scores[i].score < newScore.score){
                PersonalScore temp = new PersonalScore(scores[i].score, scores[i].time, scores[i].kills, scores[i].name);
                scores[i] = newScore;
                InsertScore(temp, startingIndex+1);
                return i;
            }
            if(scores[i].score == newScore.score && scores[i].time > newScore.time){
                PersonalScore temp = new PersonalScore(scores[i].score, scores[i].time, scores[i].kills, scores[i].name);
                scores[i] = newScore;
                InsertScore(temp, startingIndex+1);
                return i;
            }
        }

        return -1;
    }
}

[System.Serializable] public class Settings{
    public float masterVolume, sfxVolume, musicVolume;
    public int fov;
    public int graphicsQuality;

    public Settings(float _master, float _sfx, float _music, int _fov, int _graphics){
        masterVolume = _master;
        sfxVolume = _sfx;
        musicVolume = _music;
        fov = _fov;
        graphicsQuality = _graphics;
    }
}
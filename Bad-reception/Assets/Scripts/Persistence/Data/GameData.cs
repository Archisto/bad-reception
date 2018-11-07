using System;
using System.Collections.Generic;

namespace Persistence
{
    [Serializable]
    public class GameData : ISaveData
    {
        public int ID { get; set; }
        //public List<string> TaskQuestions = new List<string>();
        //public List<List<string>> TaskAnswers = new List<List<string>>();
        //public List<int> TaskCorrectAnswers = new List<int>();
        public List<SerializableTask> PlayerTasks = new List<SerializableTask>();
    }
}

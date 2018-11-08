using System;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class SerializableTask
    {
        public string Question;
        public List<string> Answers;
        public int CorrectAnswer;
        public string AudioClipTag;

        
        public SerializableTask(string question, List<string> answers, int correctAnswer, string audioClipTag)
        {
            Question = question;
            Answers = answers;
            CorrectAnswer = correctAnswer;
            AudioClipTag = audioClipTag;
        }

        public SerializableTask(PlayerTask playerTask)
        {
            Question = playerTask.question;
            Answers = playerTask.answers;
            CorrectAnswer = playerTask.correctAnswer;
           
        }

        public static implicit operator SerializableTask(PlayerTask playerTask)
        {
            return new SerializableTask(playerTask);
        }

        public static implicit operator PlayerTask(SerializableTask serializableTask)
        {
            PlayerTask playerTask = new PlayerTask(serializableTask.Question);
            playerTask.SetAnswers(serializableTask.CorrectAnswer, serializableTask.Answers);
           
            return playerTask;
        }

        /// <summary>
        /// Every class in C# has ToString method and it can be overridden like this.
        /// </summary>
        /// <returns>String representation of the class</returns>
        public override string ToString()
        {
            return string.Format(Answers.ToString());
        }
    }
}

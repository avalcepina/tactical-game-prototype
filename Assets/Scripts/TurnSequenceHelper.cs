
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class TurnSequenceHelper
    {

        public static List<Character> GetCharacterSequence(Character[] characters)
        {

            List<Character> result = new List<Character>();

            Array.Sort<Character>(characters, new Comparison<Character>(
                              (i1, i2) => i2.initiative.CompareTo(i1.initiative)));

            List<Character> charactersList = new List<Character>(characters);

            String lastTeam = "";

            while (charactersList.Count > 0)
            {

                Character character = charactersList[0];

                if (result.Count == 0 || lastTeam != character.team)
                {
                    result.Add(character);
                    charactersList.RemoveAt(0);
                    lastTeam = character.team;

                }
                else
                {
                    int index = GetCharacterIndexForRemoval(charactersList, lastTeam);

                    Character nextCharacter = charactersList[index];

                    result.Add(nextCharacter);
                    charactersList.RemoveAt(index);
                    lastTeam = nextCharacter.team;

                }

            }

            return result;

        }

        static int GetCharacterIndexForRemoval(List<Character> charactersList, String lastTeam)
        {

            for (int i = 0; i < charactersList.Count; i++)
            {
                if (charactersList[i].team != lastTeam)
                {
                    return i;
                }


            }

            return 0;

        }

    }


}
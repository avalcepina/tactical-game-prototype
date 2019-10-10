using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SA;
using System.Linq;
using UnityEngine;

namespace Tests
{
    public class NewTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {

            Character[] characters = new Character[5];
            characters[0] = BuildCharacter("char1", "team1", 1);
            characters[1] = BuildCharacter("char2", "team1", 10);
            characters[2] = BuildCharacter("char3", "team2", 8);
            characters[3] = BuildCharacter("char4", "team1", 11);
            characters[4] = BuildCharacter("char5", "team2", 9);

            List<Character> expected = new List<Character>();

            expected.Add(characters[3]);
            expected.Add(characters[4]);
            expected.Add(characters[1]);
            expected.Add(characters[2]);
            expected.Add(characters[0]);

            List<Character> result = TurnSequenceHelper.GetCharacterSequence(characters);

            Assert.True(Enumerable.SequenceEqual(expected, result));

        }

        Character BuildCharacter(string name, string team, int initiative)
        {

            Character character = new GameObject().AddComponent<Character>().GetComponent<Character>();
            character.name = name;
            character.team = team;
            character.initiative = initiative;

            return character;

        }

    }
}

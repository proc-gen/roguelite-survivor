using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Utils
{
    internal class RandomTable
    {
        Dictionary<string, int> _entries;
        int _totalWeight;

        public RandomTable()
        {
            _entries = new Dictionary<string, int>();
            _totalWeight = 0;
        }

        public RandomTable Add(string name, int weight)
        {
            if (weight > 0)
            {
                _entries[name] = weight;
                _totalWeight = _entries.Values.Sum();
            }
            return this;
        }

        public string Roll(Random random)
        {
            string retVal = "None";

            if(_totalWeight > 0)
            {
                int roll = random.Next(0, _totalWeight);

                int index = 0;
                while (roll > 0)
                {
                    if(roll < _entries.ElementAt(index).Value)
                    {
                        retVal = _entries.ElementAt(index).Key;
                        roll = 0;
                    }
                    else
                    {
                        roll -= _entries.ElementAt(index).Value;
                        index++;
                    }
                }
            }

            return retVal;
        }
    }
}

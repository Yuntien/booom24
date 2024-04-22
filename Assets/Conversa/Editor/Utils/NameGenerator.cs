using System;

namespace Conversa.Editor
{
    public class NameGenerator
    {
        private readonly string baseName;
        
        private readonly Func<string, bool> isValid;

        private const int IterationLimit = 500;
        
        public NameGenerator(string baseName, Func<string, bool> isValid)
        {
            this.baseName = baseName;
            this.isValid = isValid;
        }

        public string Generate()
        {
            if (isValid(baseName)) return baseName;

            for (var i = 0; i <= IterationLimit; i++)
            {
                var candidate = $"{baseName} ({i})";
                if (isValid(candidate)) return candidate;
            }

            throw new Exception("Couldn't find a possible filename");
        }
    }
}
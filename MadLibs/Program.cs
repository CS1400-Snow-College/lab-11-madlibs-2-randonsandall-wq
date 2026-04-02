class Program
{
    static void Main(string[] args)
    {



        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run <story1.txt> <story2.txt> ...");
            return;
        }






        //builds the dictionary
        Dictionary<string, List<string>> categoryToWords = BuildDictionary(args);


        foreach (string filename in args)
        {


            GenerateStory(filename, categoryToWords);
        }

        Console.WriteLine("MadLibs stories generated successfully!");
    }




    static Dictionary<string, List<string>> BuildDictionary(string[] filenames)
    {
        Dictionary<string, List<string>> categoryToWords = new Dictionary<string, List<string>>();

        foreach (string filename in filenames)
        {


            string content = File.ReadAllText(filename);

            ParseStory(content, categoryToWords);
        }


        return categoryToWords;
    }




    static void ParseStory(string story, Dictionary<string, List<string>> categoryToWords)
    {


        int index = 0;






        while (index < story.Length)
        {




            int startPos = story.IndexOf("::", index);
            if (startPos == -1)
                break;

            //words before
            int wordStart = startPos - 1;


            while (wordStart >= 0 && !char.IsWhiteSpace(story[wordStart]))
                wordStart--;
            wordStart++;

            string word = story.Substring(wordStart, startPos - wordStart);

            //after
            int categoryStart = startPos + 2;
            int categoryEnd = categoryStart;
            while (categoryEnd < story.Length && !char.IsWhiteSpace(story[categoryEnd]) && story[categoryEnd] != '.')
                categoryEnd++;




            string category = story.Substring(categoryStart, categoryEnd - categoryStart);




    if (!categoryToWords.ContainsKey(category))
            {
                categoryToWords[category] = new List<string>();
            }

            if (!categoryToWords[category].Contains(word))
            {
                categoryToWords[category].Add(word);
            }

            index = categoryEnd;
        }
    }


    static void GenerateStory(string filename, Dictionary<string, List<string>> categoryToWords)
    {
        string content = File.ReadAllText(filename);
        Random random = new Random();
        string result = "";
        int index = 0;

        while (index < content.Length)

        {
            int startPos = content.IndexOf("::", index);

            if (startPos == -1)
            {




                result += content.Substring(index);
                break;
            }

            int wordStart = startPos - 1;
            while (wordStart >= 0 && !char.IsWhiteSpace(content[wordStart]))
                wordStart--;
            wordStart++;

            result += content.Substring(index, wordStart - index);





            int categoryStart = startPos + 2;
            int categoryEnd = categoryStart;
            while (categoryEnd < content.Length && !char.IsWhiteSpace(content[categoryEnd]) && content[categoryEnd] != '.')
                categoryEnd++;

            string category = content.Substring(categoryStart, categoryEnd - categoryStart);




            if (categoryToWords.ContainsKey(category) && categoryToWords[category].Count > 0)
            {
                int randomIndex = random.Next(categoryToWords[category].Count);
                result += categoryToWords[category][randomIndex];
            }

            index = categoryEnd;
        }


        string outputFilename = "generated." + Path.GetFileName(filename);

        File.WriteAllText(outputFilename, result);
        Console.WriteLine($"Generated: {outputFilename}");
    }
}

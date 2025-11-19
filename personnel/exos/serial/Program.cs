using System.Text.Json;

Console.WriteLine("hello");

var maya = new Character();

string json = JsonSerializer.Serialize(maya);
File.WriteAllText("character.json", json);

string data = System.IO.File.ReadAllText("character.json");

data = JsonSerializer.Deserialize<string>(json);

Console.WriteLine(json);

public class Actor
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Country { get; set; }
    public bool IsAlive { get; set; }
}

public class Character
{
    public Character()
    {
        FirstName = "Maya";
        LastName = "test";
        Description = "testtesttest";
        PlayedBy = null;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public Actor PlayedBy { get; set; }
}

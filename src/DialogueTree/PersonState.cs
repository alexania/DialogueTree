namespace DialogueTree
{
  public class PersonState
  {
    public PersonState()
    {
      Status = PersonStatus.Start;
      Mood = Mood.Neutral;
      CanEndCurrentTopic = true;
    }

    public PersonStatus Status { get; set; }

    public Mood Mood { get; set; }

    public bool CanEndCurrentTopic { get; set; }
  }

  public enum PersonStatus
  {
    Start,
    Greeted,
    Neutral,
    End,
    Any = 255
  }

  public enum Mood
  {
    Neutral,
    Good,
    Bad
  }
}

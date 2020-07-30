namespace DialogueTree
{
  public class State
  {
    private PersonState _bot;
    private PersonState _person;

    private bool _botSpeaking = true;

    public State()
    {
      _bot = new PersonState();
      _person = new PersonState();
      CurrentTopic = "";
    }

    public string CurrentTopic { get; set; }

    public PersonState Speaker
    {
      get
      {
        return _botSpeaking ? _bot : _person;
      }
    }

    public PersonState Listener
    {
      get
      {
        return _botSpeaking ? _person : _bot;
      }
    }

    public bool CanEndCurrentTopic
    {
      get
      {
        return Speaker.CanEndCurrentTopic;
      }
      set
      {
        Speaker.CanEndCurrentTopic = value;
      }
    }

    public void UpdateSpeaker(PersonStatus changeToStatus)
    {
      // Update Speaker
      if (changeToStatus != PersonStatus.Any)
      {
        Speaker.Status = changeToStatus;
      }
    }

    public void SwitchSpeaker()
    {
      _botSpeaking = !_botSpeaking;
    }
  }
}

using DialogueTree.Nodes;
using DialogueTree.Nodes.Conditions;
using DialogueTree.Nodes.Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DialogueTree
{
  public class Tree
  {
    private State _state;

    private IList<TopicNode> _nodes;

    public Tree()
    {
      _state = new State();
      _nodes = new List<TopicNode>();

      var n = CreateTopicNode("Greeting", PersonStatus.Start, PersonStatus.Greeted);
      CreateResponseNode(n, new[] { "Hi!", "Hello!" });
      CreateResponseNode(n, new[] { "Good Morning!" }).AddTimeCondition(4, 12);//.AddCondition(node => DateTime.Now.Hour >= 4 && DateTime.Now.Hour < 12);
      CreateResponseNode(n, new[] { "Good Afternoon!" }).AddTimeCondition(12, 18);//.AddCondition(node => DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18);
      CreateResponseNode(n, new[] { "Good Evening!" }).AddTimeCondition(18).AddTimeCondition(-1, 4);//.AddCondition(node => DateTime.Now.Hour >= 18 || DateTime.Now.Hour < 4);
      CreateResponseNode(n, new[] { "Good Day!" }).AddTimeCondition(4, 18);//.AddCondition(node => DateTime.Now.Hour >= 4 && DateTime.Now.Hour <= 18);

      n = CreateTopicNode("Wellness", PersonStatus.Greeted);
      CreateResponseNode(n, new[] { "How are you?", "How are you doing?", "Are you well?" }, 1);
      CreateResponseNode(n, new[] { "Good! You?", "I'm pretty good. Yourself?" }, 2).AddMoodEffect(Mood.Good);
      CreateResponseNode(n, new[] { "Eh, not bad. You?", "Doing alright, and you?" }, 2).AddMoodEffect(Mood.Neutral);
      CreateResponseNode(n, new[] { "I've been better. And you?", "Today sucks ass. Hope yours is better?" }, 2).AddMoodEffect(Mood.Bad);
      CreateResponseNode(n, new[] { "I'm alright!", "I'm pretty good." }, 3).AddMoodEffect(Mood.Good);
      CreateResponseNode(n, new[] { "Eh, not bad.", "Doing alright." }, 3).AddMoodEffect(Mood.Neutral);
      CreateResponseNode(n, new[] { "I've been better.", "Today sucks ass." }, 3).AddMoodEffect(Mood.Bad);

      var s = CreateSubTopicNode(n, "WhatsWrong", 4, PersonStatus.Greeted).AddListenerMoodCondition(Mood.Bad);
      CreateResponseNode(s, new[] { "Oh no, what's wrong?" }, 1);
      CreateResponseNode(s, new[] { "Nothing big, just having a meh day." }, 2).AddMoodEffect(Mood.Neutral);
      CreateResponseNode(s, new[] { "Ugh, you won't believe what happened. It's just been one thing after the other." }, 2);
      CreateResponseNode(s, new[] { "Ach sorry man, that sucks.", "Shame, hope it gets better." }, 3);
    }

    public void Trigger()
    {
      var response = GetValidResponse();
      var row = Console.CursorTop;

      while (response != null)
      {
        row = Send(response.Activate(_state), row);
        _state.SwitchSpeaker();

        var availableResponses = GetValidResponses();
        var responseId = WaitForResponse(availableResponses);

        if (responseId == null)
        {
          Console.WriteLine("\n");
          return;
        }

        var r = availableResponses[responseId.Item1];
        row = Send(r.Activate(_state, responseId.Item2), row, false);
        _state.SwitchSpeaker();

        response = GetValidResponse();
      }
    }

    private ResponseNode CreateResponseNode(TopicNode n, string[] responses, int topicDepth = 0)
    {
      return CreateNode(n, new ResponseNode(responses), topicDepth);
    }

    private TopicNode CreateSubTopicNode(TopicNode n, string topic, int topicDepth = 0, PersonStatus validStatus = PersonStatus.Any, PersonStatus changeToStatus = PersonStatus.Any)
    {
      return CreateNode(n, new TopicNode(topic, validStatus, changeToStatus), topicDepth);
    }

    private TopicNode CreateTopicNode(string topic, PersonStatus validStatus = PersonStatus.Any, PersonStatus changeToStatus = PersonStatus.Any)
    {
      var n = new TopicNode(topic, validStatus, changeToStatus);
      _nodes.Add(n);
      return n;
    }

    public T CreateNode<T>(TopicNode parent, T child, int topicDepth = 0) where T : BaseNode
    {
      parent.AddChild(topicDepth, child);
      return child;
    }

    public ResponseNode GetValidResponse()
    {
      if (_nodes.Count > 0)
      {
        var responses = _nodes.Select(t => t.GetValidResponse(null, _state)).Where(t => t != null).ToList();
        if (responses.Count > 0)
        {
          return Utils.GetRandomFromList(responses);
        }
      }
      return null;
    }

    public IList<ResponseNode> GetValidResponses()
    {
      if (_nodes.Count > 0)
      {
        var responses = new List<ResponseNode>();

        foreach (var node in _nodes)
        {
          var r = node.GetValidResponses(null, _state);
          if (r != null && r.Count > 0)
          {
            responses.AddRange(r);
          }
        }
        return responses;
      }
      return new List<ResponseNode>();
    }

    private static int Send(string text, int row, bool alignLeft = true)
    {
      ClearToRow(row);

      if (!alignLeft)
      {
        for (var i = 0; i < 40 - text.Length; i++)
        {
          Console.Write($" ");
        }
      }
      for (var i = 0; i < text.Length; i++)
      {
        Console.Write($"{text[i]}");
        Thread.Sleep(50);
      }
      Console.WriteLine("\n");

      return Console.CursorTop;
    }

    private static void ClearToRow(int row)
    {
      while (Console.CursorTop > row)
      {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
      }
    }

    private Tuple<int, int> WaitForResponse(IList<ResponseNode> availableResponses)
    {
      var responseKeys = new List<Tuple<int, int>>();

      for (var i = 0; i < availableResponses.Count; i++)
      {
        for (var j = 0; j < availableResponses[i].Responses.Count; j++)
        {
          Console.WriteLine($"  {responseKeys.Count}. {availableResponses[i].Responses[j]}");
          responseKeys.Add(new Tuple<int, int>(i, j));
        }
      }
      Console.WriteLine("\n");

      var response = Console.ReadKey();
      while (response.Key != ConsoleKey.Escape)
      {
        var key = (int)response.Key;
        if (key >= 96 && key < 96 + responseKeys.Count)
        {
          return responseKeys[key - 96];
        }
        response = Console.ReadKey();
      }
      return null;
    }
  }
}

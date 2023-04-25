using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

public class ChatReader : MonoBehaviour
{
    private const string messageExpression = @"badges=(.+?);.*display\-name=(.+?);.*user\-id=(.+?);.*:(.*)!.*PRIVMSG.+?:(.*)";
    private const string rewardExpression = @"badges=(.+?);.*custom\-reward\-id=(.+?);.*display\-name=(.+?);.*user\-id=(.+?);.*:(.*)!.*PRIVMSG.+?:(.*)";

    [SerializeField] private Config gameConfig;
    [SerializeField] private Command[] commands;
    public static event Action<ChatCommand> CommandExecuted;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer; 
    private float reconnectTimer;
    private float reconnectAfter; 

    void Start()
    {
        reconnectAfter = 60.0f;
        Connect();   
    }

    void Update()
    {
        if (twitchClient.Available == 0)
            reconnectTimer += Time.deltaTime;

        if (twitchClient.Available == 0 && twitchClient.Connected && reconnectTimer >= reconnectAfter)
        { 
            Connect(); 
            reconnectTimer = 0.0f;
        }

        ReadChat();
    }

    private void Connect()
    {
        if (twitchClient != null && twitchClient.Connected)
            return;

        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine($"PASS {StreamerData.password}");
        writer.WriteLine($"NICK {StreamerData.username}");
        writer.WriteLine($"JOIN #{gameConfig.channelName}");

        writer.WriteLine("CAP REQ :twitch.tv/tags");
        writer.WriteLine("CAP REQ :twitch.tv/commands");
        writer.WriteLine("CAP REQ :twitch.tv/membership");

        writer.Flush(); 
    }

    public void ReadChat()
    {
        if (twitchClient != null && twitchClient.Available > 0)
        {
            string unformattedMessage = reader.ReadLine();
            if (unformattedMessage.Contains("PRIVMSG"))
            {
                ChatMessage chatMessage = formatMessage(unformattedMessage, unformattedMessage.Contains("custom-reward-id"));
                Command executed = getCommand(chatMessage.message);
                if (executed != null)
                {
                    ChatCommand chatCommand = new ChatCommand(chatMessage, executed, unformattedMessage.Contains("custom-reward-id"));
                    CommandExecuted?.Invoke(chatCommand);
                    print(chatMessage.user + ": " + chatMessage.message); 
                }
            }
        }
    }

    private ChatMessage formatMessage(string command, bool isReward)
    {
        Regex messageRegex = new Regex((isReward)? rewardExpression : messageExpression);
        string chatName = messageRegex.Match(command).Groups[(isReward)? 5 : 4].Value;
        string message = messageRegex.Match(command).Groups[(isReward)? 6 : 5].Value;

        ChatMessage chatMessage = new ChatMessage();
        chatMessage.user = chatName;
        chatMessage.message = message.ToLower();
        return chatMessage;
    }

    private Command getCommand(string message)
    {
        foreach (Command c in commands)
        {
            if (message.StartsWith("!" + c.CommandName + " ") || message.Equals("!" + c.CommandName))
                return c;
            foreach (string s in c.Aliases)
                if (message.StartsWith("!" + s + " ") || message.Equals("!" + s))
                    return c;
        }
        return null;
    }

    private void OnApplicationQuit() => CloseTCP();

    private void OnDestroy() => CloseTCP();

    private void CloseTCP()
    {
        if (twitchClient == null) return;
            try
            {
                twitchClient.Close();
                twitchClient.Dispose();
                twitchClient = null;
            }
            catch { }
    }
}
[System.Serializable]
public class ChatCommand
{
    public Command command;
    public string username;
    public bool isReward;

    public ChatCommand(ChatMessage message, Command submittedCommand, bool reward)
    {
        command = submittedCommand;
        username = message.user;
        isReward = reward;
    }
}
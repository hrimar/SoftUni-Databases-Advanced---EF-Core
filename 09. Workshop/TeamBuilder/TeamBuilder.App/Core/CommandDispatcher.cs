using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamBuilder.App.Core.Commands;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = string.Empty;

            string[] inputArgs = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string commandName = inputArgs.Length > 0 ? inputArgs[0] : string.Empty;
            inputArgs = inputArgs.Skip(1).ToArray();

           
            switch (commandName.ToLower())
            {
                case "exit":
                    ExitCommand exit = new ExitCommand();
                    exit.Execute(inputArgs);
                    break;
                case "registeruser":
                    RegisterUserCommand registerUser = new RegisterUserCommand();
                    result = registerUser.Execute(inputArgs);
                    break;
                case "login":
                    LoginCommand loginUser = new LoginCommand();
                    result = loginUser.Execute(inputArgs);
                    break;
                case "logout":
                    LogoutCommand logoutUser = new LogoutCommand();
                    result = logoutUser.Execute(inputArgs);
                    break;
                case "deleteuser":
                    DeleteUserCommand deleteUser = new DeleteUserCommand();
                    result = deleteUser.Execute(inputArgs);
                    break;
                case "createevent":
                    CreateEventCommand createEvent = new CreateEventCommand();
                    result = createEvent.Execute(inputArgs);                   
                    break;
                case "createteam":
                    CreateTeamCommand createTeam = new CreateTeamCommand();
                    result = createTeam.Execute(inputArgs);
                    break;
                case "showevent":
                    ShowEventCommand showEvent = new ShowEventCommand();
                    result = showEvent.Execute(inputArgs);
                    break;
                case "invitetoteam":
                    InviteToTeamCommand inviteToTeam = new InviteToTeamCommand();
                    result = inviteToTeam.Execute(inputArgs);
                    break;
                case "showteam":
                    ShowTeamCommand showTeam = new ShowTeamCommand();
                    result = showTeam.Execute(inputArgs);
                    break;
                case "acceptinvite":
                   AcceptInviteCommand acceptInvite = new AcceptInviteCommand();
                    result = acceptInvite.Execute(inputArgs);
                    break;
                case "declineinvite":
                    DeclineInviteCommand declineInvite = new DeclineInviteCommand();
                    result = declineInvite.Execute(inputArgs);
                    break;
                case "kickmember":
                    KickMemberCommand kickMember = new KickMemberCommand();
                    result = kickMember.Execute(inputArgs);
                    break;
                case "disband":
                    DisbandCommand disband = new DisbandCommand();
                    result = disband.Execute(inputArgs);
                    break;
                case "addteamto":
                    AddTeamToCommand addTeamTo = new AddTeamToCommand();
                    result = addTeamTo.Execute(inputArgs);
                    break;

                default:
                    throw new NotSupportedException($"Command {commandName} not supported!");
            }

            return result;
        }
    }
}

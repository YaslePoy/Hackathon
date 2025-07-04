using Fluxor;
using HackathonApi.Models;

namespace HackathonWeb;

[FeatureState]
public class HackState
{
    public User? User { get; set; }
    public string? Token { get; set; }
}

public static class HackReducer
{
    [ReducerMethod]
    public static HackState Setup(HackState state, SetupUserAction action)
    {
        return new HackState { User = action.User, Token = action.Token };
    }

    [ReducerMethod]
    public static HackState LogOut(HackState state, LogOutAction action)
    {
        return new HackState();
    }
}

public record SetupUserAction(User User, string Token);

public record LogOutAction;